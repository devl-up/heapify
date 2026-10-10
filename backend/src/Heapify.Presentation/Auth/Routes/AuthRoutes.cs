using Heapify.Domain.Users.Entities;
using Heapify.Presentation.Auth.DTO;
using Heapify.Presentation.Auth.Extensions;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Heapify.Presentation.Auth.Routes;

internal static class AuthRoutes
{
    extension(IEndpointRouteBuilder app)
    {
        internal void MapAuthRoutes()
        {
            var group = app.MapGroup("auth")
                .WithTags("Auth");

            group.MapGet("me",
                async (HttpContext http, [FromServices] UserManager<User> userManager) =>
                {
                    var id = http.User.GetUserId();

                    if (!id.HasValue)
                    {
                        return Results.Unauthorized();
                    }

                    var user = await userManager.FindByIdAsync(id.Value.ToString());

                    return user == null
                        ? Results.Unauthorized()
                        : Results.Ok(new UserDto(user.UserName));
                });

            group.MapPost("register", async (
                [FromServices] UserManager<User> userManager,
                [FromBody] RegisterDto dto) =>
            {
                var user = new User
                {
                    Id = Guid.CreateVersion7(),
                    UserName = dto.Username,
                    Email = dto.Email
                };

                var result = await userManager.CreateAsync(user, dto.Password);

                return result.Succeeded ? Results.Ok() : Results.BadRequest();
            }).AllowAnonymous();

            group.MapPost("login", async (
                [FromServices] SignInManager<User> signInManager,
                [FromServices] UserManager<User> userManager,
                [FromBody] LoginDto dto) =>
            {
                var user = await userManager.FindByEmailAsync(dto.Email);
                if (user == null)
                {
                    return Results.Unauthorized();
                }

                var result = await signInManager.PasswordSignInAsync(user, dto.Password, true, false);

                return result.Succeeded ? Results.Ok() : Results.Unauthorized();
            }).AllowAnonymous();

            group.MapPost("logout", async ([FromServices] SignInManager<User> signInManager) =>
            {
                await signInManager.SignOutAsync();
                return Results.Ok();
            });

            group.MapGet("antiforgery", ([FromServices] IAntiforgery antiforgery, HttpContext http) =>
            {
                var tokens = antiforgery.GetAndStoreTokens(http);

                http.Response.Cookies.Append(
                    "XSRF-TOKEN",
                    tokens.RequestToken!,
                    new CookieOptions
                    {
                        HttpOnly = false,
                        Secure = true,
                        SameSite = SameSiteMode.Strict
                    });

                return Results.NoContent();
            }).AllowAnonymous();
        }
    }
}