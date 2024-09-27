using GearUp.Models;
using GearUp.Models.RequestModels;
using GearUp.Models.ResponseModels;
using GearUp.Services.IService;
using Microsoft.AspNetCore.Mvc;

namespace GearUp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<UserResponseModel>>> AddUser(UserRequestModel user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<UserResponseModel>
                {
                    StatusCode = 400,
                    Message = "Validation failed.",
                    Data = null
                });
            }

            var response = await _userService.CreateUserAsync(user);
            return StatusCode(response.StatusCode, new ApiResponse<UserResponseModel>
            {
                StatusCode = response.StatusCode,
                Message = response.Message,
                Data = response.Data,
            });
        }

    }
}
