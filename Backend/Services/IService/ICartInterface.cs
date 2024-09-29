using GearUp.Models.ResponseModels;
using GearUp.Models;
using Microsoft.AspNetCore.Mvc;

namespace GearUp.Services.IService
{
    public interface ICartInterface
    {
        Task<ActionResult<ApiResponse<IEnumerable<CartItemResponseModel>>>> GetAllCartItems(int userId);
        Task<ActionResult<ApiResponse<CartItemResponseModel>>> AddItemToCartAsync(int productId, int userId, int quantity);
        Task<ActionResult<ApiResponse<CartItemResponseModel>>> UpdateItemQuantityAsync(int productId, int userId, int quantity);
        Task<ActionResult<ApiResponse<CartItemResponseModel>>> RemoveItemFromCartAsync(int productId, int userId, int quantity);
        Task<ActionResult<ApiResponse<CartItemResponseModel>>> DeleteItemFromCartAsync(int productId, int userId);
    }
}
