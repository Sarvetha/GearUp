using System.ComponentModel.DataAnnotations;

namespace GearUp.Models
{
    public class Cart
    {
        [Key]
        public int CartId { get; set; }
        public int UserId { get; set; }
        public List<CartItem> Items { get; set; }
        public decimal TotalAmount { get; set; }
        public User User { get; set; }
    }
}
