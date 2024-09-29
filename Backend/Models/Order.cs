using System.ComponentModel.DataAnnotations;

namespace GearUp.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public int CartId { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime OrderDateTime { get; set; }
        public string DeliveryAddress { get; set; }
        public User User { get; set; }
        public Cart Cart { get; set; }
    }
}
