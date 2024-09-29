namespace GearUp.Models.ResponseModels
{
    public class CartItemsResponseModel
    {
        public int CartItemId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public Product Product { get; set; }
    }
}
