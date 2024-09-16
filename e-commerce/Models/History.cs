using System.ComponentModel.DataAnnotations.Schema;

namespace e_commerce.Models
{
    public class History
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        [ForeignKey(nameof(Customer))]
        public string CustomerId { get; set; }
        public ApplicationUser Customer { get; set; }
    }
}
