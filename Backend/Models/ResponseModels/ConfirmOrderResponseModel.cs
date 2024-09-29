namespace GearUp.Models.ResponseModels
{
    public class ConfirmOrderResponseModel
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime OrderDateTime { get; set; }
        public string Address { get; set; }
    }
}
