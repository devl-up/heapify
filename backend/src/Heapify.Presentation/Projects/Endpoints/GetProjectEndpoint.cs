using System.Net;
using System.Security.Claims;
using Heapify.Application.Projects.Queries;
using Heapify.Presentation.Common.Endpoints;
using Heapify.Presentation.Common.Enums;
using Heapify.Presentation.Common.Requirements;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Heapify.Presentation.Projects.Endpoints;

internal sealed class GetProjectEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder builder)
    {
        builder.MapGet("projects/{id:guid}", HandleAsync)
            .Produces<GetProject.Response>()
            .Produces((int)HttpStatusCode.Forbidden)
            .WithTags("Projects");
    }

    private static async Task<IResult> HandleAsync(
        ClaimsPrincipal user,
        IAuthorizationService authorizationService,
        GetProject.Handler handler,
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var authorizationResult =
            await authorizationService.AuthorizeAsync(user, id,
                new PermissionRequirement(ProjectPermission.Read));

        if (!authorizationResult.Succeeded)
        {
            return Results.Forbid();
        }

        var request = new GetProject.Request
        {
            Id = id
        };

        var response = await handler.HandleAsync(request, cancellationToken);

        return Results.Ok(response);
    }
}