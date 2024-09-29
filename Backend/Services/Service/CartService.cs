using GearUp.Models;
using GearUp.Models.ResponseModels;
using GearUp.Services.IService;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace GearUp.Services.Service
{
    public class CartService : ICartInterface
    {
        private readonly GearUpContext _context;
        public CartService(GearUpContext context)
        {
            _context = context;
        }

        public async Task<ActionResult<ApiResponse<IEnumerable<CartItemResponseModel>>>> GetAllCartItems(int userId)
        {
            var userExists = await _context.Users.AnyAsync(u => u.UserId == userId);

            if (!userExists)
            {
                return new ApiResponse<IEnumerable<CartItemResponseModel>>
                {
                    StatusCode = 404,
                    Message = "User not found.",
                    Data = null
                };
            }

            var cartItems = await _context.Carts.Where(c => c.UserId == userId).Include(c => c.Items).ThenInclude(ci => ci.Product).ToListAsync();

            if (cartItems == null || !cartItems.Any())
            {
                return new ApiResponse<IEnumerable<CartItemResponseModel>>
                {
                    StatusCode = 404,
                    Message = "Cart is empty or does not exist.",
                    Data = null
                };
            }

            var cartItemResponseModels = cartItems.Select(item => new CartItemResponseModel
            {
                UserId = userId,
                Items = item.Items,
            }).ToList();


            return new ApiResponse<IEnumerable<CartItemResponseModel>>
            {
                StatusCode = 200,
                Message = "Cart items retrieved successfully.",
                Data = cartItemResponseModels
            };
        }

        public async Task<ActionResult<ApiResponse<CartItemResponseModel>>> AddItemToCartAsync(int productId, int userId, int quantity)
        {
            var userExists = await _context.Users.AnyAsync(u => u.UserId == userId);
            if (!userExists)
            {
                return new ApiResponse<CartItemResponseModel>
                {
                    StatusCode = 404,
                    Message = "User not found.",
                    Data = null
                };
            }

            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                return new ApiResponse<CartItemResponseModel>
                {
                    StatusCode = 404,
                    Message = "Product not found.",
                    Data = null
                };
            }

            if (product.Stock < quantity)
            {
                return new ApiResponse<CartItemResponseModel>
                {
                    StatusCode = 400,
                    Message = "Not enough stock available.",
                    Data = null
                };
            }

            var cartItem = await _context.CartItems.Include(i => i.User).Include(i => i.Product).FirstOrDefaultAsync(c => c.UserId == userId && c.ProductId == productId);

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    if (cartItem != null)
                    {
                        cartItem.Quantity += quantity;
                        _context.CartItems.Update(cartItem);
                    }
                    else
                    {
                        cartItem = new CartItem
                        {
                            UserId = userId,
                            ProductId = productId,
                            Quantity = quantity
                        };

                        await _context.CartItems.AddAsync(cartItem);

                        // Check if a cart already exists for the user
                        var existingCart = await _context.Carts
                            .FirstOrDefaultAsync(c => c.UserId == userId);

                        Cart cart;
                        if (existingCart != null)
                        {
                            cart = existingCart;
                            if (cartItem != null)
                            {
                                cartItem.CartId = cart.CartId; // Set the CartId
                                cartItem.Quantity += quantity; // Update the quantity
                                _context.CartItems.Update(cartItem); // Mark as modified
                            }
                        }
                        else
                        {
                            cart = new Cart
                            {
                                UserId = userId,
                                Items = new List<CartItem> { cartItem },
                                TotalAmount = 0
                            };

                            await _context.Carts.AddAsync(cart);
                            await _context.SaveChangesAsync();

                            var cartid = cart.CartId;

                            cartItem.CartId = cartid;
                            _context.CartItems.Update(cartItem);
                        }
                    }

                    product.Stock -= quantity;
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return new ApiResponse<CartItemResponseModel>
                    {
                        StatusCode = 200,
                        Message = "Items added to the cart",
                        Data = new CartItemResponseModel
                        {
                            UserId = userId,
                            Items = new List<CartItem> { cartItem }
                        }
                    };
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return new ApiResponse<CartItemResponseModel>
                    {
                        StatusCode = 500,
                        Message = "An error occurred while adding items to the cart.",
                        Data = null
                    };
                }
            }
        }

        public async Task<ActionResult<ApiResponse<CartItemResponseModel>>> UpdateItemQuantityAsync(int productId, int userId, int quantity)
        {
            var cartItem = await _context.CartItems.Include(c => c.Product).FirstOrDefaultAsync(c => c.UserId == userId && c.ProductId == productId);
            if (cartItem == null) 
            {
                return new ApiResponse<CartItemResponseModel>
                {
                    StatusCode = 404,
                    Message = "Cart item not found.",
                    Data = null
                };
            }
            else
            {
                cartItem.Quantity += quantity;
                cartItem.Product.Stock -= quantity;
                _context.SaveChanges();

                return new ApiResponse<CartItemResponseModel>
                {
                    StatusCode = 200,
                    Message = "Items added to the cart",
                    Data = new CartItemResponseModel
                    {
                        UserId = userId,
                        Items = new List<CartItem> { cartItem }
                    }
                };
            }
        }

        public async Task<ActionResult<ApiResponse<CartItemResponseModel>>> RemoveItemFromCartAsync(int productId, int userId, int quantity)
        {
            var cartItem = await _context.CartItems.Include(c => c.Product).FirstOrDefaultAsync(c => c.UserId == userId && c.ProductId == productId);
            
            if (cartItem == null)
            {
                return new ApiResponse<CartItemResponseModel>
                {
                    StatusCode = 404,
                    Message = "Cart item not found.",
                    Data = null
                };
            }

            if (cartItem.Quantity <= quantity)
            {
                _context.CartItems.Remove(cartItem);
            }
            else
            {
                cartItem.Quantity -= quantity;
            }

            cartItem.Product.Stock += quantity;
            _context.SaveChanges();

            return new ApiResponse<CartItemResponseModel>
            {
                StatusCode = 200,
                Message = "Items removed from the cart",
                Data = new CartItemResponseModel
                {
                    UserId = userId,
                    Items = new List<CartItem> { cartItem }
                }
            };
        }

        public async Task<ActionResult<ApiResponse<CartItemResponseModel>>> DeleteItemFromCartAsync(int productId, int userId)
        {
            var cartItem = await _context.CartItems.Include(c => c.Product).FirstOrDefaultAsync(c => c.UserId == userId && c.ProductId == productId);
            if (cartItem == null)
            {
                return new ApiResponse<CartItemResponseModel>
                {
                    StatusCode = 404,
                    Message = "Cart item not found.",
                    Data = null
                };
            }

            _context.CartItems.Remove(cartItem);
            cartItem.Product.Stock += cartItem.Quantity;
            _context.SaveChanges();

            return new ApiResponse<CartItemResponseModel>
            {
                StatusCode = 200,
                Message = "Cart item deleted successfully.",
                Data = null
            };
        }
    }
}