namespace GearUp.Models.ResponseModels
{
    public class OrderDetailsResponseModel
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public List<CartItem> Items { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime OrderDateTime { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
    }
}
