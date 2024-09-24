using e_commerce.Models;

namespace e_commerce.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetProductsBySupplierId(string supplierId);
    }
}
