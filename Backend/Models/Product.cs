using System.ComponentModel.DataAnnotations;

namespace GearUp.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string ImageURL { get; set; }

        // Navigation property for the cart
        public ICollection<Cart> Carts { get; set; }
    }
}
