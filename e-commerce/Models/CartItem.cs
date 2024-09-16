using System.ComponentModel.DataAnnotations.Schema;

namespace e_commerce.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        [ForeignKey(nameof(User))]
        public string UserId { get; set; }
        [ForeignKey(nameof(Product))]
        public int ProductId { get; set; }
        public ApplicationUser User { get; set; }
        public Product Product { get; set; }
    }
}
