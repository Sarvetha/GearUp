namespace GearUp.Models.RequestModels
{
    public class ConfirmOrderRequestModel
    {
        public int userId  { get; set; }
        public int cartId { get; set; }
        public string address { get; set; }
        public string PhoneNumber { get; set; }
     }
}
