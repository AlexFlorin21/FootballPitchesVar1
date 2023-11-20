using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity;
namespace Football.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Username is required.")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }
    public class RegisterModel
    {
        [Required(ErrorMessage = "Numele de utilizator este obligatoriu.")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Adresa de email este obligatorie.")]
        [EmailAddress(ErrorMessage = "Adresa de email nu este validă.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Parola este obligatorie.")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Compare("Password", ErrorMessage = "Parolele nu se potrivesc.")]
        [DataType(DataType.Password)]
        public string? ConfirmPassword { get; set; }
    }
    public class User :IdentityUser
    {

        public string? FullName { get; set; }

        // Timestamp-uri pentru audit
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int UpdatedByUserId { get; set; } // ID-ul utilizatorului care a efectuat ultima actualizare

        public string? ResetToken { get; set; } // Token pentru resetarea parolei
        public DateTime? ResetTokenExpiration { get; set; } // Data de expirare a tokenului pentru resetarea parolei

        // Relația One-to-Many cu Booking
        public ICollection<Booking> Bookings { get; set; }

        // Relația Many-to-Many cu Field
        public ICollection<UserField> UserFields { get; set; }

        // Proprietate pentru relația One-to-One cu Coupon
        public Coupon? Coupon { get; set; }

        public virtual ICollection<Review> Reviews { get; set; }

        // Constructor
        public User()
        {
            UserFields = new HashSet<UserField>();
            Bookings = new HashSet<Booking>();
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
            Reviews = new HashSet<Review>();
        }



        // Exemplu de metodă pentru resetarea parolei sau alte operațiuni specifice utilizatorului
        public void InitiatePasswordReset()
        {
            // Generarea unui token pentru resetarea parolei
            ResetToken = GenerateResetToken();

            // Setarea datei de expirare a tokenului (de exemplu, la 1 oră de la inițiere)
            ResetTokenExpiration = DateTime.Now.AddHours(1);
        }

        // Metodă pentru resetarea parolei
        public void ResetPassword(string newPassword)
        {
            if (CanResetPassword())
            {
                // Resetarea efectivă a parolei (de exemplu, hashingul noii parole și salvarea ei)
                PasswordHash = HashPassword(newPassword);

                // Resetarea tokenului și datei de expirare
                ResetToken = null;
                ResetTokenExpiration = null;
            }
        }

        // Metoda pentru verificarea dacă poate fi resetată parola
        public bool CanResetPassword()
        {
            return ResetTokenExpiration.HasValue && ResetTokenExpiration > DateTime.Now;
        }

        // Metodă pentru generarea unui token pentru resetarea parolei (simplificată)
        private string GenerateResetToken()
        {
            return Convert.ToBase64String(Guid.NewGuid().ToByteArray()) +
                   Convert.ToBase64String(RandomNumberGenerator.GetBytes(16));
        }

        // Metodă pentru hashingul parolei (simplificată)
        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
        public bool VerifyPassword(string inputPassword, string storedHash)
        {
            return BCrypt.Net.BCrypt.Verify(inputPassword, storedHash);
        }
    }
}