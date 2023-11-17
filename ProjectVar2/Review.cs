using System.ComponentModel.DataAnnotations;

namespace Football.Models
{
    public class Review
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; } // ID-ul utilizatorului care a scris review-ul

        [Required]
        public string? Text { get; set; }

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        // Proprietatea de navigație pentru User
        public User? User { get; set; }

        public Review()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }
    }
}
