namespace e_commerce.Models
{
    public class CartItemDto
    {
        public int CartItemId { get; set; }
        public string ProductName { get; set; }
        public string ProductImageURL { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
