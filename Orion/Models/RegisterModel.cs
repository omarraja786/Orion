using System.ComponentModel.DataAnnotations;

namespace Orion.Models
{
    public class RegisterModel
    {

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }


        public string Organization { get; set; }

    }
}
