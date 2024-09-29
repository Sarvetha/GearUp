using Azure;
using GearUp.Models;
using GearUp.Models.RequestModels;
using GearUp.Models.ResponseModels;
using GearUp.Services.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GearUp.Services.Service
{
    public class OrderService : IOrderService
    {
        private readonly GearUpContext _context;
        private readonly EmailService _emailService;
        public OrderService(GearUpContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public async Task<ActionResult<ApiResponse<OrderDetailsResponseModel>>> GetOrderAsync(int userId)
        {
            var response = new ApiResponse <OrderDetailsResponseModel> ();
            var orderDetails = await _context.Orders.FindAsync(userId);
            var cart = await _context.Carts.Include(c => c.Items).FirstOrDefaultAsync(c => c.UserId == userId);

            if (orderDetails == null)
            {
                response.StatusCode = 404;
                response.Message = "Order not found.";
                response.Data = null;
            }
            else
            {
                response.StatusCode = 200;
                response.Message = "Product retrieved successfully.";
                response.Data = new OrderDetailsResponseModel
                {
                    OrderId = orderDetails.OrderId,
                    UserId = userId,
                    Items = cart?.Items ?? new List<CartItem>(),
                    TotalAmount = orderDetails.TotalAmount,
                    OrderDateTime = orderDetails.OrderDateTime,
                    Address = orderDetails.User.Address,
                    PhoneNumber = orderDetails.User.PhoneNumber
                };
            }

            return response;
        }

        public async Task<ActionResult<ApiResponse<ConfirmOrderResponseModel>>> CheckoutOrderAsync(ConfirmOrderRequestModel confirmOrderModel, bool confirmOrder)
        {
            var response = new ApiResponse<ConfirmOrderResponseModel>();
            var userDetails = await _context.Users.FindAsync(confirmOrderModel.userId);

            try
            {
                if (confirmOrder)
                {
                    decimal totalAmount = await CalculateTotalAmount(confirmOrderModel.cartId);
                    var order = new Order
                    {
                        UserId = confirmOrderModel.userId,
                        CartId = confirmOrderModel.cartId,
                        TotalAmount = totalAmount,
                        OrderDateTime = DateTime.Now,
                        DeliveryAddress = confirmOrderModel.address
                    };

                    _context.Orders.Add(order);
                    await _context.SaveChangesAsync();

                    string subject = "Order Confirmation";
                    string message = $"<h1>Thank you for your order!</h1>" +
                                     $"<p>Hello {userDetails.FirstName},</p>" +
                                     $"<p>Your order has been successfully placed.</p>" +
                                     $"<p>Total Amount: {totalAmount:C}</p>" +
                                     $"<p>Delivery Address: {confirmOrderModel.address}</p>";

                    await _emailService.SendEmailAsync(userDetails.Email, subject, message);

                    response.StatusCode = 200;
                    response.Message = "Order Confirmed";
                    response.Data = new ConfirmOrderResponseModel
                    {
                        UserId = confirmOrderModel.userId,
                        Name = userDetails.FirstName,
                        TotalAmount = totalAmount,
                        OrderDateTime = DateTime.Now,
                        Address = confirmOrderModel.address
                    };
                }
            }
            catch(Exception ex)
            {
                response.StatusCode = 500; // Internal Server Error
                response.Message = "An unexpected error occurred: " + ex.Message;
                response.Data = null;
            }
            return response;
        }

        public async Task<decimal> CalculateTotalAmount(int cartId)
        {
            var cartItems = await _context.Carts.Include(c => c.Items).ThenInclude(ci => ci.Product).Where(ci => ci.CartId == cartId).ToListAsync();

            decimal totalAmount = 0;

            foreach (var cart in cartItems)
            {
                foreach (var item in cart.Items)
                {
                    var product = item.Product; 

                    if (product != null)
                    {
                        totalAmount += item.Quantity * product.Price; 
                    }
                }
            }

            return totalAmount;
        }

    }
}
