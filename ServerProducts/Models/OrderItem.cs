namespace Products24Backend.Models
{
    public class OrderItem
    {
        public Guid OrderItemID { get; set; }

        public Guid OrderID { get; set; }
        public Order Order { get; set; } = null!;

        public Guid ProductID { get; set; }
        public Product Product { get; set; } = null!;

        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
