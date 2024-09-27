using GearUp.Models;
using GearUp.Models.RequestModels;
using GearUp.Models.ResponseModels;
using GearUp.Services.IService;
using Microsoft.EntityFrameworkCore;

namespace GearUp.Services.Service
{
    public class UserService : IUserService
    {
        private readonly GearUpContext _context;

        public UserService(GearUpContext context) 
        { 
            _context = context;
        }

        public async Task<ApiResponse<UserResponseModel>> CreateUserAsync(UserRequestModel user)
        { 
            var response = new ApiResponse<UserResponseModel>();

            try
            {
                var addUser = new User 
                { 
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Address = user.Address
                };

                _context.Users.Add(addUser);
                await _context.SaveChangesAsync();

                response.StatusCode = 201; // Created
                response.Message = "User created successfully.";
                response.Data = new UserResponseModel
                {
                    Id = addUser.UserId,
                    FirstName = addUser.FirstName,
                    LastName = addUser.LastName,
                    Email = addUser.Email,
                    Address = addUser.Address
                };
            }
            catch (DbUpdateException ex)
            {
                response.StatusCode = 500; // Internal Server Error
                response.Message = "Database error occurred while creating user. " + ex.Message;
                response.Data = null;
            }
            catch (Exception ex)
            {
                response.StatusCode = 500; // Internal Server Error
                response.Message = "An unexpected error occurred: " + ex.Message;
                response.Data = null;
            }
            return response;
        }

    }
}
