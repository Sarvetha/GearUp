using GearUp.Models;
using GearUp.Models.ResponseModels;
using GearUp.Services.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GearUp.Services.Service
{
    public class CartService : ICartInterface
    {
        private readonly GearUpContext _context;

        public CartService(GearUpContext context)
        {
            _context = context;
        }

        public async Task<ActionResult<ApiResponse<IEnumerable<Cart>>>> GetAllCartItemsAsync(int userId)
        {
            // Check if the user exists
            var userExists = await _context.Users.AnyAsync(u => u.UserId == userId);

            if (!userExists)
            {
                return new ApiResponse<IEnumerable<Cart>>
                {
                    StatusCode = 404,
                    Message = "User not found.",
                    Data = null
                };
            }

            var cartItems = await _context.Carts
                                .Where(c => c.UserId == userId)
                                .Include(c => c.Product)
                                .ToListAsync();

            if (cartItems == null || !cartItems.Any())
            {
                return new ApiResponse<IEnumerable<Cart>>
                {
                    StatusCode = 404,
                    Message = "Cart is empty or does not exist.",
                    Data = new List<Cart>()
                };
            }

            return new ApiResponse<IEnumerable<Cart>>
            {
                StatusCode = 200,
                Message = "Cart items retrieved successfully.",
                Data = cartItems
            };
        }

        public async Task<ActionResult<ApiResponse<Cart>>> AddProductToCartAsync(int userId, int productId, int quantity)
        {
            var userExists = await _context.Users.AnyAsync(u => u.UserId == userId);
            if (!userExists)
            {
                return new ApiResponse<Cart>
                {
                    StatusCode = 404,
                    Message = "User not found.",
                    Data = null
                };
            }

            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                return new ApiResponse<Cart>
                {
                    StatusCode = 404,
                    Message = "Product not found.",
                    Data = null
                };
            }

            var cartItem = await _context.Carts.FirstOrDefaultAsync(c => c.UserId == userId && c.ProductId == productId);

            if (cartItem != null)
            {
                // Update the quantity if the product is already in the cart
                cartItem.Quantity += quantity;
                product.Stock -= quantity;
            }
            else
            {
                // Create a new cart entry if the product is not in the cart
                cartItem = new Cart
                {
                    UserId = userId,
                    ProductId = productId,
                    Quantity = quantity,
                    UnitPrice = product.Price
                };
                product.Stock -= quantity;
                await _context.Carts.AddAsync(cartItem);
            }

            await _context.SaveChangesAsync();

            return new ApiResponse<Cart>
            {
                StatusCode = 200,
                Message = "Product added to cart successfully.",
                Data = cartItem
            };
        }

    }
}