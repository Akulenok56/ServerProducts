namespace Products24Backend.DTO
{
    public class CreateOrderDto
    {
        public Guid UserID { get; set; }
        public List<OrderItemDto> Items { get; set; } = new();

        public class OrderItemDto
        {
            public Guid ProductID { get; set; }
            public int Quantity { get; set; }
        }
    }
}
