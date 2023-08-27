using System.ComponentModel.DataAnnotations;

namespace Orion.Data
{
    public class UserData
    {
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Organization { get; set; }

        [Required]
        public bool IsAdmin { get; set; }

        [Required]
        public bool IsLoggedIn { get; set; }
    }
}
