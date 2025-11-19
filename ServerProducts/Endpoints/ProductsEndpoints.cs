using Products24Backend.Data;
using Products24Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Products24Backend.Endpoints
{
    public static class ProductEndpoints
    {
        public static void MapProductEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/products");

            group.MapGet("/", async (AppDbContext db) => await db.Products.ToListAsync());
            group.MapGet("/{id}", async (Guid id, AppDbContext db) =>
                await db.Products.FindAsync(id) is Product p ? Results.Ok(p) : Results.NotFound());

            group.MapPost("/", async (Product product, AppDbContext db) =>
            {
                await db.Products.AddAsync(product);
                await db.SaveChangesAsync();
                return Results.Created($"/products/{product.ProductID}", product);
            });

            
        }
    }
}
