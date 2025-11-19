using Microsoft.EntityFrameworkCore;
using Products24Backend.Models;
using ServerProducts.Models;

namespace Products24Backend.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<Cart> Carts => Set<Cart>();
        public DbSet<CartItem> CartItems => Set<CartItem>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasPostgresExtension("pgcrypto");

            modelBuilder.Entity<User>()
                .Property(x => x.UserID)
                .HasDefaultValueSql("gen_random_uuid()");

            modelBuilder.Entity<Product>()
                .Property(x => x.ProductID)
                .HasDefaultValueSql("gen_random_uuid()");

            modelBuilder.Entity<Cart>()
                .Property(x => x.CartID)
                .HasDefaultValueSql("gen_random_uuid()");

            modelBuilder.Entity<CartItem>()
                .Property(x => x.CartItemID)
                .HasDefaultValueSql("gen_random_uuid()");

            modelBuilder.Entity<Order>()
                .Property(x => x.OrderID)
                .HasDefaultValueSql("gen_random_uuid()");

            modelBuilder.Entity<OrderItem>()
                .Property(x => x.OrderItemID)
                .HasDefaultValueSql("gen_random_uuid()");

            
            modelBuilder.Entity<User>()
                .HasMany(u => u.Orders)
                .WithOne(o => o.User)
                .HasForeignKey(o => o.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .HasMany(u => u.CollectedOrders)
                .WithOne(o => o.Collector)
                .HasForeignKey(o => o.CollectorID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Carts)
                .WithOne(c => c.User)
                .HasForeignKey(c => c.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            
            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Cart)
                .WithMany(c => c.CartItems)
                .HasForeignKey(ci => ci.CartID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Product)
                .WithMany(p => p.CartItems)
                .HasForeignKey(ci => ci.ProductID)
                .OnDelete(DeleteBehavior.Restrict);

        
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Product)
                .WithMany(p => p.OrderItems)
                .HasForeignKey(oi => oi.ProductID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
