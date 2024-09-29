using GearUp.Models;
using GearUp.Models.RequestModels;
using GearUp.Models.ResponseModels;
using GearUp.Services.IService;
using GearUp.Services.Service;
using Microsoft.AspNetCore.Mvc;

namespace GearUp.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        [Route("GetOrder")]
        public async Task<ActionResult<ApiResponse<OrderDetailsResponseModel>>> GetOrder(int userId)
        {
            var response = await _orderService.GetOrderAsync(userId);
            return response;
        }

        [HttpPost]
        [Route("CheckoutOrder")]
        public async Task<ActionResult<ApiResponse<ConfirmOrderResponseModel>>> CheckoutOrder(ConfirmOrderRequestModel confirmOrderModel, bool confirmOrder)
        {
            if (confirmOrder == false)
            {
                return BadRequest("Invalid order confirmation details.");
            }

            var confirmation = await _orderService.CheckoutOrderAsync(confirmOrderModel, confirmOrder);

            return confirmation;
        }
    }
}
