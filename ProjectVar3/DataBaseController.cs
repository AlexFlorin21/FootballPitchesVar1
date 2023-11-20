using Football.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Football.Controllers
{
    public class DataBaseController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DataBaseController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Exemplu de metodă pentru a obține rezervările pentru o anumită zi
        public IActionResult GetBookingsForDate(DateTime date)
        {
            var bookings = _context.Bookings
                           .Where(b => b.StartTime.Date == date.Date)
                           .ToList();

            // Returnează un View, JSON, sau orice alt tip de răspuns adecvat
            return View(bookings);
        }

        //Gruparea Recenziilor (Review) pe Baza Rating-ului:
        public IActionResult GroupReviewsByRating()
        {
            var groupedReviews = _context.Reviews
                                 .GroupBy(r => r.Rating)
                                 .Select(group => new { Rating = group.Key, Reviews = group.ToList() })
                                 .ToList();

            return View(groupedReviews); // sau return Json(groupedReviews) pentru un API
        }

        // Asocierea Rezervărilor (Booking) cu Utilizatorii Corespunzători (User):
        public IActionResult GetBookingsWithUsers()
        {
            var bookingsWithUsers = _context.Bookings
                                    .Include(booking => booking.User) // Presupunând că există o proprietate User în Booking
                                    .ToList();

            return View(bookingsWithUsers); // sau return Json(bookingsWithUsers) pentru un API
        }

        //Încărcarea Rezervărilor (Booking) cu Terenurile Corespunzătoare (Field):
        public IActionResult GetBookingsWithFields()
        {
            var bookingsWithFields = _context.Bookings
                                     .Include(b => b.Field)
                                     .ToList();

            return View(bookingsWithFields); // sau return Json(bookingsWithFields) pentru un API
        }

        //  găsirea tuturor utilizatorilor care au făcut rezervări la un anumit teren și au un rating mediu peste 4.
        public IActionResult GetUsersWithHighRatingsAtField(int fieldId)
        {
            var utilizatoriCuRatingInalt = _context.Users
                .Include(u => u.Bookings)
                .Where(u => u.Bookings.Any(b => b.FieldId == fieldId))
                .Select(u => new { User = u, AverageRating = u.Reviews.Any() ? u.Reviews.Average(r => r.Rating) : 0 })
                .Where(u => u.AverageRating > 4)
                .ToList();

            return View(utilizatoriCuRatingInalt); // sau return Json(utilizatoriCuRatingInalt) pentru un API
        }
    }
}
