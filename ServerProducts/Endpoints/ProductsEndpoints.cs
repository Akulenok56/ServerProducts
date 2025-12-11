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

            group.MapPost("/with-image", async ( HttpRequest request, AppDbContext db, IWebHostEnvironment env) =>
                        {
                            var form = await request.ReadFormAsync();

                            var name = form["name"].ToString();
                            var description = form["description"].ToString();
                            var price = decimal.Parse(form["price"]);
                            var stock = int.Parse(form["stockQuantity"]);

                            var file = form.Files["image"];
                            if (file == null || file.Length == 0)
                                return Results.BadRequest("Картинка обязательна");

                            var ext = Path.GetExtension(file.FileName).ToLower();
                            var fileName = $"{Guid.NewGuid()}{ext}";
                            var savePath = Path.Combine(env.WebRootPath, "images", fileName);

                            using (var stream = new FileStream(savePath, FileMode.Create))
                            {
                                await file.CopyToAsync(stream);
                            }

                           
                            var imageUrl = $"http://10.0.2.2:5162/images/{fileName}";

                            var product = new Product
                            {
                                ProductID = Guid.NewGuid(),
                                Name = name,
                                Description = description,
                                Price = price,
                                StockQuantity = stock,
                                ImageUrl = imageUrl
                            };

                            db.Products.Add(product);
                            await db.SaveChangesAsync();

                            return Results.Ok(product);
            });





            group.MapPut("/{id}/add-stock", async (Guid id, AddStockDto dto, AppDbContext db) =>
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
