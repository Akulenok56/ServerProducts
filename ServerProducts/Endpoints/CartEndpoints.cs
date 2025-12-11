using Products24Backend.Data;
using ServerProducts.DTO;
using Microsoft.EntityFrameworkCore;
using Products24Backend.Models;  
using ServerProducts.Models;
using System.Security.Claims;

namespace ServerProducts.Endpoints
{
    public static class CartEndpoints
    {
        public static void MapCartEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/cart");


            group.MapGet("/items", async (HttpContext context, AppDbContext db) =>
            {
                var userIdStr = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userIdStr == null)
                    return Results.Unauthorized();

                var userId = Guid.Parse(userIdStr);

                var cart = await db.Carts
                    .Include(c => c.CartItems)
                    .ThenInclude(i => i.Product)
                    .FirstOrDefaultAsync(c => c.UserID == userId);

                if (cart == null)
                    return Results.Ok(new List<CartItemDto>());

                var items = cart.CartItems.Select(ci => new CartItemDto
                {
                    CartItemID = ci.CartItemID,
                    ProductID = ci.ProductID,
                    ProductName = ci.Product.Name,
                    Price = ci.Product.Price,
                    Quantity = ci.Quantity,
                    ImageUrl = ci.Product.ImageUrl
                });

                return Results.Ok(items);
            })
 .RequireAuthorization();


            group.MapPost("/items", async ( HttpContext context, AddCartItemDto dto, AppDbContext db) =>
            {
                var userIdStr = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (userIdStr == null)
                    return Results.Unauthorized();

                var userId = Guid.Parse(userIdStr);

                var user = await db.Users
                    .Include(u => u.Carts)
                    .ThenInclude(c => c.CartItems)
                    .FirstOrDefaultAsync(u => u.UserID == userId);

                if (user == null)
                    return Results.NotFound("Пользователь не найден");

                var product = await db.Products.FindAsync(dto.ProductID);
                if (product == null)
                    return Results.NotFound("Товар не найден");

                if (product.StockQuantity < dto.Quantity)
                    return Results.BadRequest("Недостаточно товара");

                var cart = user.Carts.FirstOrDefault();
                if (cart == null)
                {
                    cart = new Cart
                    {
                        CartID = Guid.NewGuid(),
                        UserID = userId
                    };

                    user.Carts.Add(cart);
                    db.Carts.Add(cart);
                }

                var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductID == dto.ProductID);

                if (cartItem != null)
                {
                    cartItem.Quantity += dto.Quantity;
                }
                else
                {
                    cartItem = new CartItem
                    {
                        CartItemID = Guid.NewGuid(),
                        CartID = cart.CartID,
                        ProductID = product.ProductID,
                        Quantity = dto.Quantity
                    };

                    cart.CartItems.Add(cartItem);
                    db.CartItems.Add(cartItem);
                }

                await db.SaveChangesAsync();

                return Results.Ok(new CartItemDto
                {
                    CartItemID = cartItem.CartItemID,
                    ProductID = cartItem.ProductID,
                    ProductName = product.Name,
                    Price = product.Price,
                    Quantity = cartItem.Quantity,
                    ImageUrl = product.ImageUrl
                });
            })
 .RequireAuthorization();

            
            group.MapDelete("/{userId}/items/{itemId}", async (Guid userId, Guid itemId, AppDbContext db) =>
            {
                var cartItem = await db.CartItems
                    .Include(ci => ci.Cart)
                    .FirstOrDefaultAsync(ci => ci.CartItemID == itemId && ci.Cart.UserID == userId);

                if (cartItem == null)
                    return Results.NotFound("Элемент корзины не найден");

                db.CartItems.Remove(cartItem);
                await db.SaveChangesAsync();
                return Results.Ok("Товар удален из корзины");
            });

            group.MapPut("/items/{itemId}/increase", async (HttpContext context, Guid itemId, AppDbContext db) =>
            {
                var userIdStr = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userIdStr == null)
                    return Results.Unauthorized();

                var userId = Guid.Parse(userIdStr);

                var cartItem = await db.CartItems
                    .Include(ci => ci.Cart)
                    .Include(ci => ci.Product)
                    .FirstOrDefaultAsync(ci =>
                        ci.CartItemID == itemId &&
                        ci.Cart.UserID == userId);

                if (cartItem == null)
                    return Results.NotFound("Товар в корзине не найден");

                cartItem.Quantity++;
                await db.SaveChangesAsync();

                return Results.Ok(new CartItemDto
                {
                    CartItemID = cartItem.CartItemID,
                    ProductID = cartItem.ProductID,
                    ProductName = cartItem.Product.Name,
                    Price = cartItem.Product.Price,
                    Quantity = cartItem.Quantity,
                    ImageUrl = cartItem.Product.ImageUrl
                });
            }).RequireAuthorization();



            group.MapPut("/items/{itemId}/decrease", async (HttpContext context, Guid itemId, AppDbContext db) =>
            {
                var userIdStr = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userIdStr == null)
                    return Results.Unauthorized();

                var userId = Guid.Parse(userIdStr);

                var cartItem = await db.CartItems
                    .Include(ci => ci.Cart)
                    .Include(ci => ci.Product)
                    .FirstOrDefaultAsync(ci =>
                        ci.CartItemID == itemId &&
                        ci.Cart.UserID == userId);

                if (cartItem == null)
                    return Results.NotFound("Товар в корзине не найден");

                
                if (cartItem.Quantity == 1)
                {
                    db.CartItems.Remove(cartItem);
                }
                else
                {
                    cartItem.Quantity--;
                }

                await db.SaveChangesAsync();
                return Results.Ok();
            }).RequireAuthorization();
        }

    }
}
