using System.ComponentModel.DataAnnotations;

namespace e_commerce.Models
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        public bool isSupplier { get; set; }  // Determine if the user is a Supplier or Customer

        [DataType(DataType.Upload)]
        public IFormFile? Image { get; set; }
    }
}
