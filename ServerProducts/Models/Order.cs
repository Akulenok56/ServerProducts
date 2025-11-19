namespace Products24Backend.Models
{
    public class Order
    {
        public Guid OrderID { get; set; }

        public Guid UserID { get; set; }
        public User User { get; set; } = null!;

        public Guid? CollectorID { get; set; }
        public User? Collector { get; set; }

        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = string.Empty;

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
