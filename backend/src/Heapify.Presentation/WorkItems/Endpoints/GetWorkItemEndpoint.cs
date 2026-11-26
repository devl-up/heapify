using System.Net;
using System.Security.Claims;
using Heapify.Application.WorkItems.Queries;
using Heapify.Presentation.Common.Endpoints;
using Heapify.Presentation.Common.Enums;
using Heapify.Presentation.Common.Requirements;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Heapify.Presentation.WorkItems.Endpoints;

internal sealed class GetWorkItemEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder builder)
    {
        builder.MapGet("projects/{id:guid}/work-items/{number:long}", HandleAsync)
            .Produces<GetWorkItem.Response>()
            .Produces((int)HttpStatusCode.Forbidden)
            .WithTags("Projects");
    }

    private static async Task<IResult> HandleAsync(
        ClaimsPrincipal user,
        IAuthorizationService authorizationService,
        GetWorkItem.Handler handler,
        [FromRoute] Guid id,
        [FromRoute] long number,
        CancellationToken cancellationToken)
    {
        var authorizationResult =
            await authorizationService.AuthorizeAsync(user, id,
                new PermissionRequirement(ProjectPermission.Read));

        if (!authorizationResult.Succeeded)
        {
            return Results.Forbid();
        }

        var request = new GetWorkItem.Request
        {
            ProjectId = id,
            Number = number
        };

        var response = await handler.HandleAsync(request, cancellationToken);
        return Results.Ok(response);
    }
}