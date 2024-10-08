using e_commerce.Models;
using e_commerce.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace e_commerce.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly CartService _cartService;
        private readonly UserManager<ApplicationUser> _userManager;

        public CartController(CartService cartService , UserManager<ApplicationUser> userManager)
        {
            _cartService = cartService;
            _userManager = userManager;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddToCart([FromForm] AddToCartRequest request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var user = await _userManager.FindByEmailAsync(userId);

            userId = user.Id;

            if (userId == null) return Unauthorized();

            await _cartService.AddToCart(userId, request.ProductId, request.Quantity);
            return Ok(new { message = "Item added to cart" });
        }

        [HttpGet]
        public async Task<IActionResult> GetCartItems()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var user = await _userManager.FindByEmailAsync(userId);

            userId = user.Id;

            if (userId == null) return Unauthorized();

            var cartItems = await _cartService.GetCartItems(userId);
            return Ok(cartItems);
        }

        [HttpDelete("remove/{productId}")]
        public async Task<IActionResult> RemoveFromCart(int productId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var user = await _userManager.FindByEmailAsync(userId);

            userId = user.Id;

            if (userId == null) return Unauthorized();

            await _cartService.RemoveFromCart(userId, productId);
            return Ok(new { message = "Item removed from cart" });
        }
    }
}
