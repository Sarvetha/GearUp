using System.ComponentModel.DataAnnotations;

namespace GearUp.Models.RequestModels
{
    public class UserRequestModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [EmailAddress(ErrorMessage = "Please Mention Valid Email")]
        public string Email { get; set; }
        public string Address { get; set; }
    }
}
