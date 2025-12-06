namespace Products24Backend.Models
{
    public class User
    {
        public Guid UserID { get; set; }

        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;

        public string City { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;
        public string HouseNumber { get; set; } = string.Empty;
        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public ICollection<Order> CollectedOrders { get; set; } = new List<Order>();
        public ICollection<Cart> Carts { get; set; } = new List<Cart>();
    }

}
