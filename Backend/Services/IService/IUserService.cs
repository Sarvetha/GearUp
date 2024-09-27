using GearUp.Models;
using GearUp.Models.RequestModels;
using GearUp.Models.ResponseModels;

namespace GearUp.Services.IService
{
    public interface IUserService
    {
        Task<ApiResponse<UserResponseModel>> CreateUserAsync(UserRequestModel user);
    }
}
