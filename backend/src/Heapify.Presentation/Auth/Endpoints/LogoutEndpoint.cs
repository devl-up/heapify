using System.Net;
using Heapify.Domain.Users.Entities;
using Heapify.Presentation.Common.Endpoints;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Heapify.Presentation.Auth.Endpoints;

internal sealed class LogoutEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder builder)
    {
        builder.MapPost("auth/logout", HandleAsync)
            .Produces((int)HttpStatusCode.OK)
            .WithTags("Auth");
    }

    private static async Task<IResult> HandleAsync(
        [FromServices] SignInManager<User> signInManager)
    {
        await signInManager.SignOutAsync();
        return Results.Ok();
    }
}