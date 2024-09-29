using GearUp.Models;
using GearUp.Models.RequestModels;
using GearUp.Models.ResponseModels;
using Microsoft.AspNetCore.Mvc;

namespace GearUp.Services.IService
{
    public interface IOrderService
    {
        Task<ActionResult<ApiResponse<OrderDetailsResponseModel>>> GetOrderAsync(int userId);
        Task<ActionResult<ApiResponse<ConfirmOrderResponseModel>>> CheckoutOrderAsync(ConfirmOrderRequestModel confirmOrderModel, bool confirmOrder);
    }
}
