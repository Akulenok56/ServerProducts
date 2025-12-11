namespace ServerProducts.DTO
{
    public class CartItemDto
    {
        public Guid CartItemID { get; set; }
        public Guid ProductID { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string? ImageUrl { get; set; }
    }
}
