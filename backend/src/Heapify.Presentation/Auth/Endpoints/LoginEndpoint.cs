using System.Net;
using Heapify.Domain.Users.Entities;
using Heapify.Presentation.Common.Endpoints;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Heapify.Presentation.Auth.Endpoints;

internal sealed class LoginEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder builder)
    {
        builder.MapPost("auth/login", HandleAsync)
            .AllowAnonymous()
            .Produces((int)HttpStatusCode.OK)
            .Produces((int)HttpStatusCode.Unauthorized)
            .WithTags("Auth");
    }

    private static async Task<IResult> HandleAsync(
        [FromServices] SignInManager<User> signInManager,
        [FromServices] UserManager<User> userManager,
        [FromBody] Request request)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            return Results.Unauthorized();
        }

        var result = await signInManager.PasswordSignInAsync(user, request.Password, true, false);

        return result.Succeeded ? Results.Ok() : Results.Unauthorized();
    }

    public sealed class Request
    {
        public required string Email { get; init; }
        public required string Password { get; init; }
    }
}