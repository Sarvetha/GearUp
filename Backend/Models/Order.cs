using System.ComponentModel.DataAnnotations;

namespace GearUp.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public DateTime OrderDateTime { get; set; }
        public decimal TotalAmount { get; set; }

        // Navigation property
        public User User { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
    }
}
