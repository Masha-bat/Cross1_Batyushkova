using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Batyushkova_lr1.Data;
using Batyushkova_lr1.Models;
using Microsoft.AspNetCore.Authorization;

namespace Batyushkova_lr1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TablesController : ControllerBase
    {
        private readonly Batyushkova_lr1Context _context;

        public TablesController(Batyushkova_lr1Context context)
        {
            _context = context;
        }

        // GET: api/Tables
        // список всех столов
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Table>>> GetTable()
        {
            return await _context.Table.ToListAsync();
        }

        // GET: api/Tables/5
        // стол по id
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Table>> GetTable(int id)
        {
            var table = await _context.Table.FindAsync(id);

            if (table == null)
            {
                return NotFound();
            }

            return table;
        }

        // PUT: api/Tables/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        // обновление данных стола
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutTable(int id, Table table)
        {
            if (id != table.Id)
            {
                return BadRequest();
            }

            _context.Entry(table).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TableExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Tables
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        // добавление нового стола
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Table>> PostTable(Table table)
        {
            _context.Table.Add(table);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTable", new { id = table.Id }, table);
        }

        // DELETE: api/Tables/5
        // удаление стола
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTable(int id)
        {
            var table = await _context.Table.FindAsync(id);
            if (table == null)
            {
                return NotFound();
            }

            _context.Table.Remove(table);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PUT: api/Tables/{id}/free
        // Метод для освобождения стола
        [HttpPut("{id}/free")]
        [Authorize]
        public async Task<IActionResult> FreeTable(int id)
        {
            // Ищем стол по ID
            var table = await _context.Table.FindAsync(id);
            if (table == null)
            {
                return NotFound(new { message = $"Стол с ID {id} не найден." });
            }

            // Проверяем, занят ли стол
            if (!table.IsOccupied)
            {
                return BadRequest(new { message = $"Стол с ID {id} уже свободен." });
            }

            // Вызываем метод FreeTable для освобождения стола
            table.FreeTable();

            // Сохраняем изменения в базе данных
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Ошибка при освобождении стола: {ex.Message}");
            }

            // Возвращаем успешный ответ
            return Ok(new
            {
                message = $"Стол с ID {id} успешно освобождён.",
                TableStatus = new { table.Id, table.IsOccupied }
            });
        }


        // проверка существованиея
        private bool TableExists(int id)
        {
            return _context.Table.Any(e => e.Id == id);
        }
    }
}
