namespace e_commerce.Models
{
    public class UpdateProfileViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public IFormFile ProfileImage { get; set; } // For the image
    }
}
