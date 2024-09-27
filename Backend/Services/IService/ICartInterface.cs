using GearUp.Models.ResponseModels;
using GearUp.Models;
using Microsoft.AspNetCore.Mvc;

namespace GearUp.Services.IService
{
    public interface ICartInterface
    {
        Task<IEnumerable<CartItemResponseModel>> GetCartItemsAsync(int userId);
        Task<CartItemResponseModel> AddItemToCartAsync(int productId, int userId);
        Task<CartItemResponseModel> UpdateItemQuantityAsync(int productId, int userId, int quantity);
        Task<bool> RemoveItemFromCartAsync(int productId, int userId);
    }
}
