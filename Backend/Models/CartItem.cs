using System.ComponentModel.DataAnnotations;

namespace GearUp.Models
{
    public class CartItem
    {
        [Key]
        public int CartItemId { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public Product Product { get; set; }
        public int CartId { get; set; }
        public User User { get; set; }

    }
}
