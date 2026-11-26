using System.Net;
using Heapify.Domain.Users.Entities;
using Heapify.Presentation.Common.Endpoints;
using Heapify.Presentation.Common.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Heapify.Presentation.Auth.Endpoints;

internal sealed class MeEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder builder)
    {
        builder.MapGet("auth/me", HandleAsync)
            .Produces<Response>()
            .Produces((int)HttpStatusCode.Unauthorized)
            .WithTags("Auth");
    }

    private static async Task<IResult> HandleAsync(
        HttpContext http,
        [FromServices] UserManager<User> userManager)
    {
        var id = http.User.GetUserId();

        if (!id.HasValue)
        {
            return Results.Unauthorized();
        }

        var user = await userManager.FindByIdAsync(id.Value.ToString());

        return user == null
            ? Results.Unauthorized()
            : Results.Ok(new Response { Name = user.UserName! });
    }

    public sealed class Response
    {
        public required string Name { get; init; }
    }
}