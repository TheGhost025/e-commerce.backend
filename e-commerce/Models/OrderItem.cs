using System.ComponentModel.DataAnnotations.Schema;

namespace e_commerce.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        [ForeignKey(nameof(Order))]
        public int OrderId { get; set; }
        public Order Order { get; set; }
        [ForeignKey(nameof (Product))]
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
