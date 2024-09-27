using System.ComponentModel.DataAnnotations;

namespace GearUp.Models
{
    public class Cart
    {
        [Key]
        public int CartId { get; set; }
        public int ProductId { get; set; }
        public int UserId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        // Navigation properties
        public Product Product { get; set; }
        public User User { get; set; }
    }
}
