using e_commerce.Context;
using e_commerce.Models;
using Microsoft.EntityFrameworkCore;

namespace e_commerce.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;

        public ProductService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetProductsBySupplierId(string supplierId)
        {
            return await _context.Products.Where(p => p.SupplierId == supplierId).ToListAsync();
        }
    }

}
