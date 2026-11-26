using System.Net;
using Heapify.Presentation.Common.Endpoints;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;

namespace Heapify.Presentation.Auth.Endpoints;

internal sealed class AntiforgeryEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder builder)
    {
        builder.MapGet("auth/antiforgery", HandleAsync)
            .AllowAnonymous()
            .Produces((int)HttpStatusCode.NoContent)
            .WithTags("Auth");
    }

    private static IResult HandleAsync(
        [FromServices] IAntiforgery antiforgery,
        HttpContext http)
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
    }
}