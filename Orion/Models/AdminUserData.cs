using System.ComponentModel.DataAnnotations;

namespace Orion.Models
{
    public class AdminUserData
    {
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

    }
}
