using e_commerce.Context;
using e_commerce.Models;
using Microsoft.EntityFrameworkCore;

namespace e_commerce.Services
{
    public class CartService
    {
        private readonly AppDbContext _context;

        public CartService(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddToCart(string userId, int productId, int quantity)
        {
            var existingCartItem = await _context.CartItems
                .FirstOrDefaultAsync(c => c.UserId == userId && c.ProductId == productId); 
            if (existingCartItem != null)
            {
                // If the item already exists, update the quantity
                existingCartItem.Quantity += quantity;
            }
            else
            {
                // Otherwise, add a new item to the cart
                var cartItem = new CartItem
                {
                    UserId = userId,
                    ProductId = productId,
                    Quantity = quantity
                };

                await _context.CartItems.AddAsync(cartItem);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<CartItemDto>> GetCartItems(string userId)
        {
            return await _context.CartItems
                                 .Where(c => c.UserId == userId)
                                 .Select(c => new CartItemDto
                                 {
                                     CartItemId = c.Id,
                                     ProductName = c.Product.Name,
                                     ProductImageURL = c.Product.ImageURL,
                                     Quantity = c.Quantity,
                                     Price = c.Product.Price
                                 })
                                 .ToListAsync();
        }

        public async Task RemoveFromCart(string userId, int productId)
        {
            var cartItem = await _context.CartItems
                .FirstOrDefaultAsync(c => c.UserId == userId && c.ProductId == productId);

            if (cartItem != null)
            {
                _context.CartItems.Remove(cartItem);
                await _context.SaveChangesAsync();
            }
        }
    }
}
