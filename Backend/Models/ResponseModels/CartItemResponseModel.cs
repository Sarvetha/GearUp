namespace GearUp.Models.ResponseModels
{
    public class CartItemResponseModel
    {
        public int UserId { get; set; }
        public List<CartItem> Items { get; set; }
    }
}
