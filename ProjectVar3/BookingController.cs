using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Football.Data;
using Football.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Football.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BookingController(ApplicationDbContext context)
        {
            // Constructorul injectează contextul bazei de date pentru a fi utilizat în controller.
            _context = context;
        }

        // GET: api/Booking
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Booking>>> GetAllBookings()
        {
            // Returnează toate rezervările din baza de date.
            return await _context.Bookings.ToListAsync();
        }

        // GET: api/Booking/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Booking>> GetBooking(int id)
        {
            // Caută o rezervare specifică în baza de date folosind ID-ul.
            var booking = await _context.Bookings.FindAsync(id);

            // Dacă rezervarea nu este găsită, returnează un răspuns NotFound (404).
            if (booking == null)
            {
                return NotFound();
            }

            // Returnează rezervarea găsită.
            return booking;
        }

        // POST: api/Booking
        [HttpPost]
        public async Task<ActionResult<Booking>> CreateBooking(Booking booking)
        {
            // Adaugă o nouă rezervare în baza de date.
            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            // Returnează detaliile rezervării create împreună cu un cod de status 201 (Created).
            return CreatedAtAction(nameof(GetBooking), new { id = booking.Id }, booking);
        }

        // PUT: api/Booking/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBooking(int id, Booking booking)
        {
            // Verifică dacă ID-ul din URL corespunde cu cel al rezervării.
            if (id != booking.Id)
            {
                return BadRequest();
            }

            // Marchează entitatea ca fiind modificată.
            _context.Entry(booking).State = EntityState.Modified;

            try
            {
                // Salvează modificările în baza de date.
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Verifică dacă rezervarea există în cazul unei excepții de concurență.
                if (!BookingExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            // Returnează un răspuns NoContent (204) după actualizare.
            return NoContent();
        }

        // DELETE: api/Booking/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            // Caută rezervarea care trebuie ștearsă.
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }

            // Șterge rezervarea din baza de date.
            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();

            // Returnează un răspuns NoContent (204) după ștergere.
            return NoContent();
        }

        private bool BookingExists(int id)
        {
            // Verifică dacă există o rezervare cu ID-ul specificat în baza de date.
            return _context.Bookings.Any(e => e.Id == id);
        }
    }
}
