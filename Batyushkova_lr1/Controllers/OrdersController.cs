using Batyushkova_lr1.Data;
using Batyushkova_lr1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Batyushkova_lr1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly Batyushkova_lr1Context _context;

        public OrdersController(Batyushkova_lr1Context context)
        {
            _context = context;
        }

        // GET: api/Orders
        // Получение всех заказов
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrder()
        {
            return await _context.Order
                .Include(o => o.Table)   // Включаем информацию о столе
                .Include(o => o.Dishes)  // Включаем информацию о блюдах
                .ToListAsync();
        }

        // GET: api/Orders/5
        // Получение заказа по id с вложенными данными
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _context.Order
                .Include(o => o.Table)     // Подгрузка данных о столе
                .Include(o => o.Dishes)    // Подгрузка списка блюд
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                return NotFound($"Заказ с ID {id} не найден.");
            }

            // Формирование результата для удобства
            var result = new
            {
                order.Id,
                order.TimeOrdered,
                Table = new { order.Table.Id, order.Table.Number },
                Dishes = order.Dishes.Select(d => new { d.Id, d.Name, d.Price }),
                TotalPrice = order.Dishes.Sum(d => d.Price) // Общая сумма заказа
            };

            return Ok(result);
        }


        // PUT: api/Orders/5
        // Обновление блюд в заказе
        [HttpPut("{id}/dishes")]
        [Authorize]
        public async Task<IActionResult> UpdateOrderDishes(int id, List<int> dishIds)
        {
            // Получаем заказ из базы данных вместе со списком блюд
            var order = await _context.Order
                .Include(o => o.Dishes)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                return NotFound($"Заказ с ID {id} не найден.");
            }

            // Получаем блюда по переданным dishIds
            var dishes = await _context.Dish
                .Where(d => dishIds.Contains(d.Id))
                .ToListAsync();

            if (dishes.Count != dishIds.Count)
            {
                return BadRequest("Некоторые блюда не найдены.");
            }

            // Обновляем список блюд в заказе
            order.Dishes = dishes;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                  $"Ошибка при обновлении заказа: {ex.Message}");
            }

            return Ok(new
            {
                message = "Список блюд успешно обновлён.",
                updatedDishes = order.Dishes.Select(d => new { d.Id, d.Name, d.Price })
            });
        }


        // POST: api/Orders
        // Добавление нового заказа
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Order>> PostOrder([FromBody] DTO orderDto)
        {
            // Получаем стол из базы данных
            var table = await _context.Table.FindAsync(orderDto.TableId);
            if (table == null)
            {
                return NotFound("Стол не найден");
            }

            // Получаем блюда из базы данных по id
            var dishes = await _context.Dish
                .Where(d => orderDto.DishIds.Contains(d.Id))
                .ToListAsync();

            if (dishes.Count != orderDto.DishIds.Count)
            {
                return BadRequest("Один или несколько блюд не найдены");
            }

            // Создаём новый заказ
            var order = new Order
            {
                TimeOrdered = DateTime.Now,  // Устанавливаем текущее время
                Table = table,
                Dishes = dishes
            };

            // Добавляем заказ в базу данных
            _context.Order.Add(order);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrder", new { id = order.Id }, order);
        }

        [HttpGet("table/{tableId}")]
        public async Task<ActionResult> GetOrdersByTableId(int tableId)
        {
            var orders = await _context.Order
                .Include(o => o.Dishes)
                .Include(o => o.Table)
                .Where(o => o.Table.Id == tableId)
                .ToListAsync();

            if (orders == null || !orders.Any())
            {
                return NotFound($"Заказы для стола с ID {tableId} не найдены.");
            }

            var result = orders.Select(o => new
            {
                OrderId = o.Id,
                TimeOrdered = o.TimeOrdered,
                Dishes = o.Dishes.Select(d => new
                {
                    DishName = d.Name,
                    DishPrice = d.Price
                }),
                TotalPrice = o.CalculateTotal() // Используем метод CalculateTotal
            });

            return Ok(result);
        }



        // DELETE: api/Orders/5
        // Удаление заказа
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            // Загружаем заказ с его связанными данными
            var order = await _context.Order
                .Include(o => o.Dishes)
                .Include(o => o.Table)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                return NotFound(new { message = $"Заказ с ID {id} не найден." });
            }

            // Отключаем связанные блюда от заказа (если требуется)
            order.Dishes?.Clear();

            // Удаляем заказ
            _context.Order.Remove(order);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                  $"Ошибка при удалении заказа: {ex.Message}");
            }

            // Возвращаем информацию об удалении
            return Ok(new
            {
                message = $"Заказ с ID {id} успешно удалён.",
                deletedOrder = new
                {
                    order.Id,
                    TableId = order.Table?.Id,
                    DeletedDishesCount = order.Dishes?.Count ?? 0
                }
            });
        }

        // GET: api/Orders/table/{tableId}/dish-count
        // Количество заказанных блюд на столе по ID стола
        [HttpGet("table/{tableId}/dish-count")]
        [Authorize]
        public async Task<ActionResult> GetDishCountByTableId(int tableId)
        {
            // Проверяем, есть ли стол с таким ID
            var tableExists = await _context.Table.AnyAsync(t => t.Id == tableId);
            if (!tableExists)
            {
                return NotFound($"Стол с ID {tableId} не найден.");
            }

            // Получаем все заказы для указанного стола
            var totalDishCount = await _context.Order
                .Where(o => o.Table.Id == tableId)  // Фильтруем заказы по ID стола
                .SelectMany(o => o.Dishes)         // Разворачиваем список блюд из заказов
                .CountAsync();                     // Подсчитываем общее количество блюд

            if (totalDishCount == 0)
            {
                return Ok(new { TableId = tableId, TotalDishesOrdered = totalDishCount, Message = "На этом столе нет заказов." });
            }

            return Ok(new
            {
                TableId = tableId,
                TotalDishesOrdered = totalDishCount
            });
        }





        // Проверка существования заказа
        private bool OrderExists(int id)
        {
            return _context.Order.Any(e => e.Id == id);
        }
    }
}