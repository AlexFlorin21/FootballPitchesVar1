using Football.Data;
using Football.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Football.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FieldController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public FieldController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Field>>> GetAllFields()
        {
            return await _context.Fields.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Field>> GetField(int id)
        {
            var field = await _context.Fields.FindAsync(id);
            if (field == null)
            {
                return NotFound();
            }
            return field;
        }

        [HttpPost]
        public async Task<ActionResult<Field>> CreateField(Field field)
        {
            _context.Fields.Add(field);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetField), new { id = field.Id }, field);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateField(int id, Field field)
        {
            if (id != field.Id)
            {
                return BadRequest();
            }

            _context.Entry(field).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteField(int id)
        {
            var field = await _context.Fields.FindAsync(id);
            if (field == null)
            {
                return NotFound();
            }

            _context.Fields.Remove(field);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }

}
