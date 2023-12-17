using Football.Data;
using Football.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace Football.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserFieldController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UserFieldController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserField>>> GetAllUserFields()
        {
            return await _context.UserFields.ToListAsync();
        }

        [HttpGet("{userId}/{fieldId}")]
        public async Task<ActionResult<UserField>> GetUserField(string userId, int fieldId)
        {
            var userField = await _context.UserFields
                .FirstOrDefaultAsync(uf => uf.UserId == userId && uf.FieldId == fieldId);

            if (userField == null)
            {
                return NotFound();
            }
            return userField;
        }

        [HttpPost]
        public async Task<ActionResult<UserField>> CreateUserField(UserField userField)
        {
            _context.UserFields.Add(userField);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUserField),
                new { userId = userField.UserId, fieldId = userField.FieldId }, userField);
        }

        [HttpPut("{userId}/{fieldId}")]
        public async Task<IActionResult> UpdateUserField(string userId, int fieldId, UserField userField)
        {
            if (userId != userField.UserId || fieldId != userField.FieldId)
            {
                return BadRequest();
            }

            _context.Entry(userField).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{userId}/{fieldId}")]
        public async Task<IActionResult> DeleteUserField(string userId, int fieldId)
        {
            var userField = await _context.UserFields
                .FirstOrDefaultAsync(uf => uf.UserId == userId && uf.FieldId == fieldId);

            if (userField == null)
            {
                return NotFound();
            }

            _context.UserFields.Remove(userField);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }

}
