using System.Net;
using System.Security.Claims;
using Heapify.Application.Common.DTO;
using Heapify.Application.WorkItems.Queries;
using Heapify.Presentation.Common.Endpoints;
using Heapify.Presentation.Common.Enums;
using Heapify.Presentation.Common.Requirements;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Heapify.Presentation.WorkItems.Endpoints;

internal sealed class GetWorkItemsEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder builder)
    {
        builder.MapGet("projects/{id:guid}/work-items", HandleAsync)
            .Produces<PaginatedResponse<GetWorkItems.Response>>()
            .Produces((int)HttpStatusCode.Forbidden)
            .WithTags("Projects");
    }

    private static async Task<IResult> HandleAsync(
        ClaimsPrincipal user,
        IAuthorizationService authorizationService,
        GetWorkItems.Handler handler,
        [FromRoute] Guid id,
        [FromQuery] int index,
        [FromQuery] int size,
        CancellationToken cancellationToken)
    {
        var authorizationResult =
            await authorizationService.AuthorizeAsync(user, id,
                new PermissionRequirement(ProjectPermission.Read));

        if (!authorizationResult.Succeeded)
        {
            return Results.Forbid();
        }

        var request = new GetWorkItems.Request
        {
            ProjectId = id,
            Index = index,
            Size = size
        };

        var response = await handler.HandleAsync(request, cancellationToken);
        return Results.Ok(response);
    }
}