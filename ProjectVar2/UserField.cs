using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Football.Models
{
    public class UserField
    {
        [Key, Column(Order = 0)]
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User? User { get; set; }

        [Key, Column(Order = 1)]
        public int FieldId { get; set; }
        [ForeignKey("FieldId")]
        public Field? Field { get; set; }

        // Alte proprietăți specifice relației, dacă sunt necesare
    }
}
