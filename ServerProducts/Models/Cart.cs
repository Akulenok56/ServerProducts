using ServerProducts.Models;

namespace Products24Backend.Models
{
    public class Cart
    {
        public Guid CartID { get; set; }
        public Guid UserID { get; set; }
        public User User { get; set; } = null!;

        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}
