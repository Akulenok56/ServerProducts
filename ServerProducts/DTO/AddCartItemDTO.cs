namespace ServerProducts.DTO
{
    public class AddCartItemDto
    {
        public Guid ProductID { get; set; }
        public int Quantity { get; set; }
    }
}
