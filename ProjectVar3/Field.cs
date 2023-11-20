using System.ComponentModel.DataAnnotations;

namespace Football.Models
{
    public class Field
    {
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Location { get; set; }

        public bool IsIndoor { get; set; }
        public int Capacity { get; set; }

        // Proprietăți noi adăugate
        public string? SurfaceType { get; set; } // Tipul de suprafață (iarbă naturală, artificială)
        public bool HasLighting { get; set; } // Disponibilitatea iluminării pentru jocuri nocturne
        public string? AdditionalFacilities { get; set; } // Facilități adiționale (vestiare, parcare, etc.)

        // Relația One-to-Many cu Booking
        public ICollection<Booking> Bookings { get; set; }

        // Relația Many-to-Many cu User
        public ICollection<UserField> UserFields { get; set; }

        // Constructor
        public Field()
        {
            UserFields = new HashSet<UserField>();
            Bookings = new HashSet<Booking>();
        }

        // Metode noi adăugate
        public bool CheckAvailability(DateTime requestedStart, TimeSpan duration)
        {
            DateTime requestedEnd = requestedStart.Add(duration);

            foreach (var booking in Bookings)
            {
                DateTime bookingStart = booking.StartTime;
                DateTime bookingEnd = bookingStart.Add(booking.Duration);

                // Verifică dacă există suprapunere
                if (requestedStart < bookingEnd && requestedEnd > bookingStart)
                {
                    return false; // Terenul nu este disponibil
                }
            }
            return true; // Terenul este disponibil
        }

        public decimal CalculateRentalPrice(TimeSpan duration, bool isPeakHours)
        {
            decimal baseRate = 100; // Tarif de bază pe oră, de exemplu 100 unități monetare
            decimal peakHourRate = 150; // Tarif pentru orele de vârf

            decimal rate = isPeakHours ? peakHourRate : baseRate;
            decimal totalCost = rate * (decimal)duration.TotalHours;

            return totalCost;
        }
    }
}
