using DotNet8Authentication.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DotNet8Authentication.Routes
{
    public static class UserRoutes
    {
        public static void MapUserRoutes(this IEndpointRouteBuilder app)
        {
            app.MapGet("/user/information", async (HttpContext httpContext, [FromServices] UserManager<IdentityUser> userManager) =>
            {
                var user = await userManager.GetUserAsync(httpContext.User);

                if (user == null) return Results.Unauthorized();

                var userName = user.UserName.Split('@')[0];

                return Results.Ok(new { userName, user.Email });
            })
            .RequireAuthorization()
            .WithTags("User");

            app.MapGet("/user/totalUsers", async ([FromServices] DataContext dbContext) =>
            {
                var userCount = await dbContext.Users.CountAsync(); 

                return Results.Ok(new { TotalUsers = userCount });
            })
            .WithTags("User");
        }
    }
}
