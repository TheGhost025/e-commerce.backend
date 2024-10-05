using e_commerce.Context;
using e_commerce.Models;
using e_commerce.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace e_commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IProductService _productService;

        public ProductsController(AppDbContext context, UserManager<ApplicationUser> userManager, IWebHostEnvironment webHostEnvironment,IProductService productService)
        {
            _context = context;
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
            _productService = productService;
        }

        // GET: api/Products
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts(string search = "")
        {
            // Fetch products with optional search term
            var products = await _context.Products
                .Where(p => string.IsNullOrEmpty(search) ||
                            p.Name.Contains(search) ||
                            p.Description.Contains(search))
                .ToListAsync();

            return Ok(products);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetProductsSupplier()
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var user = await _userManager.FindByEmailAsync(currentUserId);  

            var products = await _productService.GetProductsBySupplierId(user.Id);

            return Ok(products);

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct([FromForm] ProductCreateViewModel model)
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var user = await _userManager.FindByEmailAsync(currentUserId);

            currentUserId = user.Id;

            Console.WriteLine($"SupplierId: {currentUserId}");

            if (currentUserId == null)
            {
                return Unauthorized("You are not authorized to add products.");
            }

            string imageUrl = null;

            // Check if image file is included
            if (model.Image != null)
            {
                imageUrl = await SaveImageAsync(model.Image);
            }

            var product = new Product
            {
                Name = model.Name,
                Description = model.Description,
                Category = model.Category,
                Price = model.Price,
                Stock = model.Stock,
                SupplierId = currentUserId,
                ImageURL = imageUrl
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return Ok(product);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromForm] ProductCreateViewModel model)
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var user = await _userManager.FindByEmailAsync(currentUserId);

            currentUserId = user.Id;

            var product = await _context.Products.FindAsync(id);

            if (product == null || product.SupplierId != currentUserId)
            {
                return NotFound("Product not found or not authorized to edit.");
            }

            string imageUrl = product.ImageURL;

            // Check if a new image file is provided
            if (model.Image != null)
            {
                imageUrl = await SaveImageAsync(model.Image);
            }

            product.Name = model.Name;
            product.Description = model.Description;
            product.Category = model.Category;
            product.Price = model.Price;
            product.Stock = model.Stock;
            product.ImageURL = imageUrl;

            _context.Products.Update(product);
            await _context.SaveChangesAsync();

            return Ok(product);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var user = await _userManager.FindByEmailAsync(currentUserId);

            currentUserId = user.Id;

            var product = await _context.Products.FindAsync(id);

            if (product == null || product.SupplierId != currentUserId)
            {
                return NotFound("Product not found or not authorized to delete.");
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return Ok("Product deleted successfully.");
        }

        // Helper function to save image and return URL as string
        private async Task<string> SaveImageAsync(IFormFile image)
        {
            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
            Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = Guid.NewGuid().ToString() + "_" + image.FileName;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }

            return $"/images/{uniqueFileName}"; // Returning the relative URL
        }
    }
}
