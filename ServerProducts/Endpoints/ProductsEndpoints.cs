using Products24Backend.Data;
using Products24Backend.Models;
using Microsoft.EntityFrameworkCore;
using ServerProducts.DTO;

namespace Products24Backend.Endpoints
{
    public static class ProductEndpoints
    {
        public static void MapProductEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/products");

          
            group.MapGet("/", async (AppDbContext db) =>
                await db.Products.ToListAsync());

          
            group.MapGet("/{id}", async (Guid id, AppDbContext db) =>
                await db.Products.FindAsync(id) is Product p
                    ? Results.Ok(p)
                    : Results.NotFound());

           
            group.MapPost("/", async (CreateProductDto dto, AppDbContext db) =>
            {
                var product = new Product
                {
                    ProductID = Guid.NewGuid(),
                    Name = dto.Name,
                    Description = dto.Description,
                    Price = dto.Price,
                    StockQuantity = dto.StockQuantity
                };

                await db.Products.AddAsync(product);
                await db.SaveChangesAsync();

                return Results.Created($"/products/{product.ProductID}", product);
            });

         
            group.MapPut("/{id}/add-stock", async (
                Guid id,
                AddStockDto dto,
                AppDbContext db) =>
            {
                var product = await db.Products.FindAsync(id);

                if (product == null)
                    return Results.NotFound("Товар не найден");

                if (dto.QuantityToAdd <= 0)
                    return Results.BadRequest("Количество должно быть больше 0");

                product.StockQuantity += dto.QuantityToAdd;

                await db.SaveChangesAsync();

                return Results.Ok(product);
            });
        }
    }
}
