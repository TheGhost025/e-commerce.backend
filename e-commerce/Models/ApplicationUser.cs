using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;

namespace e_commerce.Models
{
    public class ApplicationUser : IdentityUser
    {
        public bool isSupplier {  get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfileImageURL { get; set; }

        [JsonIgnore]
        public ICollection<Product> Products { get; set; }
        public ICollection<CartItem> CartItems { get; set; }
        public ICollection<Order> Orders { get; set; }
        public ICollection<History> Histories { get; set; }
    }
}
