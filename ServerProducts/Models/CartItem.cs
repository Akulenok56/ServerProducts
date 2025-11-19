using Products24Backend.Models;

namespace ServerProducts.Models
{
    public class CartItem
    {
        public Guid CartItemID { get; set; }
        public Guid CartID { get; set; }
        public Cart Cart { get; set; } = null!;

        public Guid ProductID { get; set; }
        public Product Product { get; set; } = null!;

        public int Quantity { get; set; }
    }
}
