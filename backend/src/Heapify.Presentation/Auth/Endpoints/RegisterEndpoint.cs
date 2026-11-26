using System.Net;
using Heapify.Domain.Users.Entities;
using Heapify.Presentation.Common.Endpoints;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Heapify.Presentation.Auth.Endpoints;

internal sealed class RegisterEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder builder)
    {
        builder.MapPost("auth/register", HandleAsync)
            .AllowAnonymous()
            .Produces((int)HttpStatusCode.OK)
            .Produces((int)HttpStatusCode.Unauthorized)
            .WithTags("Auth");
    }

    private static async Task<IResult> HandleAsync(
        [FromServices] UserManager<User> userManager,
        [FromBody] Request request)
    {
        var user = new User
        {
            Id = Guid.CreateVersion7(),
            UserName = request.Username,
            Email = request.Email
        };

        var result = await userManager.CreateAsync(user, request.Password);

        return result.Succeeded ? Results.Ok() : Results.BadRequest();
    }

    public sealed class Request
    {
        public required string Username { get; init; }
        public required string Email { get; init; }
        public required string Password { get; init; }
    }
}