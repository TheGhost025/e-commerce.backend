using e_commerce.Context;
using e_commerce.Models;
using e_commerce.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace e_commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public PurchaseController(UserManager<ApplicationUser> userManager, AppDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [HttpPost("purchase")]
        public async Task<IActionResult> PurchaseCartItems()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Retrieve user by email
            var user = await _userManager.FindByEmailAsync(userId);
            userId = user?.Id;

            // Get cart items for the user
            var cartItems = await _context.CartItems
                .Where(c => c.UserId == userId)
                .Include(c => c.Product)
                .ToListAsync();

            if (!cartItems.Any())
            {
                return BadRequest("Your cart is empty.");
            }

            // Create a new order
            var order = new Order
            {
                CustomerId = userId,
                OrderDate = DateTime.Now,
                TotalAmount = cartItems.Sum(c => c.Product.Price * c.Quantity),
                OrderItems = cartItems.Select(c => new OrderItem
                {
                    ProductId = c.ProductId,
                    Quantity = c.Quantity,
                    Price = c.Product.Price
                }).ToList()
            };

            // Add the order to the database
            _context.Orders.Add(order);

            await _context.SaveChangesAsync();

            // Add a record to the History table to track the order status
            var history = new History
            {
                OrderId = order.Id,
                OrderDate = DateTime.Now,
                TotalAmount = cartItems.Sum(c => c.Product.Price * c.Quantity),
                CustomerId = userId
            };
            _context.Histories.Add(history);

            // Remove items from the cart
            _context.CartItems.RemoveRange(cartItems);

            await _context.SaveChangesAsync();

            return Ok(new
            {
                order.Id,
                order.CustomerId,
                OrderItems = order.OrderItems.Select(oi => new
                {
                    oi.ProductId,
                    oi.Quantity,
                    oi.Price
                }),
                history.OrderDate
            });
        }

        [HttpGet("history")]
        public async Task<IActionResult> GetPurchaseHistory()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Retrieve user by email
            var user = await _userManager.FindByEmailAsync(userId);
            userId = user?.Id;

            // Get order history for the user
            var orderHistories = await _context.Histories
                .Where(h => h.Order.CustomerId == userId)
                .Select(h => new
                {
                    h.OrderId,
                    h.OrderDate,
                    OrderItems = h.Order.OrderItems.Select(oi => new
                    {
                        oi.ProductId,
                        oi.Quantity,
                        oi.Price,
                        ProductName = oi.Product.Name,
                         ProductImageUrl = oi.Product.ImageURL
                    })
                })
                .ToListAsync();

            if (!orderHistories.Any())
            {
                return NotFound("No purchase history found.");
            }

            return Ok(orderHistories);
        }


    }
}
