using System.ComponentModel.DataAnnotations.Schema;

namespace e_commerce.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public string ImageURL { get; set; }
        public int Stock {  get; set; }
        [ForeignKey(nameof(Supplier))]
        public string SupplierId { get; set; }
        public ApplicationUser Supplier { get; set; }

        public ICollection<CartItem> CartItems { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
    }
}
