namespace e_commerce.Models
{
    public class ProductCreateViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public IFormFile Image { get; set; } // Image uploaded as file
    }
}
