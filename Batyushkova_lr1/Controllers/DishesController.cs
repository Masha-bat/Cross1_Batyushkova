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
using System.Configuration;

namespace Batyushkova_lr1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DishesController : ControllerBase
    {
        private readonly Batyushkova_lr1Context _context;

        public DishesController(Batyushkova_lr1Context context)
        {
            _context = context;
        }

        // GET: api/Dishes
        // список всех блюд
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Dish>>> GetDish()
        {
            return await _context.Dish.ToListAsync();
        }

        // GET: api/Dishes/5
        // блюдо по id
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Dish>> GetDish(int id)
        {
            var dish = await _context.Dish.FindAsync(id);

            if (dish == null)
            {
                return NotFound();
            }

            return dish;
        }

        // PUT: api/Dishes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        // обновление блюда
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutDish(int id, Dish dish)
        {
            if (id != dish.Id)
            {
                return BadRequest();
            }

            _context.Entry(dish).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DishExists(id))
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

        // POST: api/Dishes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        // добавление блюда
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Dish>> PostDish(Dish dish)
        {
            _context.Dish.Add(dish);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDish", new { id = dish.Id }, dish);
        }

        // DELETE: api/Dishes/5
        // удаление блюда
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteDish(int id)
        {
            var dish = await _context.Dish.FindAsync(id);
            if (dish == null)
            {
                return NotFound();
            }

            _context.Dish.Remove(dish);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // проверка существования блюда
        private bool DishExists(int id)
        {
            return _context.Dish.Any(e => e.Id == id);
        }
    }
}
