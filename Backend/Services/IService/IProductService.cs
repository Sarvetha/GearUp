using GearUp.Models.ResponseModels;
using GearUp.Models;

namespace GearUp.Services.IService
{
    public interface IProductService
    {
        Task<ApiResponse<IEnumerable<Product>>> GetAllProductsAsync();
        Task<ApiResponse<Product>> GetProductByIdAsync(int id);
    }
}
