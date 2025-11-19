using Products24Backend.DTO;
using Products24Backend.Services;

namespace Products24Backend.Endpoints
{
    public static class AuthEndpoints
    {
        public static void MapAuthEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/auth");

            group.MapPost("/register", async (RegisterDto dto, IAuthService auth) =>
            {
                var user = await auth.RegisterAsync(dto);
                if (user == null) return Results.Conflict(new { message = "User already exists" });
                return Results.Created($"/users/{user.UserID}", user);
            });

            group.MapPost("/login", async (LoginDto dto, IAuthService auth) =>
            {
                var token = await auth.LoginAsync(dto);
                if (token == null) return Results.Unauthorized();
                return Results.Ok(new { token });
            });
        }
    }
}
