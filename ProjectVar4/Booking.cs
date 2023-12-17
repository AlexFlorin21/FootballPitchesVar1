using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Football.Models
{
    public class Booking
    {
        public int Id { get; set; }

        [Required]
        public DateTime BookingTime { get; set; }

        // Foreign Key pentru relația cu Field
        public int FieldId { get; set; }

        // Proprietate de navigare pentru relația Many-to-One cu Field
        [ForeignKey("FieldId")]
        public Field? Field { get; set; }

        // Proprietățile pentru începutul și sfârșitul rezervării
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        // Proprietatea calculată Duration
        public TimeSpan Duration
        {
            get
            {
                return EndTime - StartTime;
            }
        }

        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        public User? User { get; set; }

        // Alte proprietăți sau metode necesare

        // Metoda pentru verificarea intervalului de timp ar putea fi adăugată aici, dacă este necesar
    }
}
