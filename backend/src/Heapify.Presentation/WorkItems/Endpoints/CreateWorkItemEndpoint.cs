using System.Net;
using System.Security.Claims;
using Heapify.Application.WorkItems.Commands;
using Heapify.Presentation.Common.Endpoints;
using Heapify.Presentation.Common.Enums;
using Heapify.Presentation.Common.Requirements;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Heapify.Presentation.WorkItems.Endpoints;

internal sealed class CreateWorkItemEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder builder)
    {
        builder.MapPost("projects/{id:guid}/work-items", HandleAsync)
            .Produces((int)HttpStatusCode.Created)
            .Produces((int)HttpStatusCode.Forbidden)
            .WithTags("Projects");
    }

    private static async Task<IResult> HandleAsync(
        ClaimsPrincipal user,
        IAuthorizationService authorizationService,
        CreateWorkItem.Handler handler,
        [FromRoute] Guid id,
        [FromBody] Request request,
        CancellationToken cancellationToken)
    {
        var authorizationResult = await authorizationService.AuthorizeAsync(user, id,
            new PermissionRequirement(ProjectPermission.Write));

        if (!authorizationResult.Succeeded)
        {
            return Results.Forbid();
        }

        var createWorkItemRequest = new CreateWorkItem.Request
        {
            ProjectId = id,
            Title = request.Title
        };

        var number = await handler.HandleAsync(createWorkItemRequest, cancellationToken);
        return Results.Created($"/api/v1/projects/{id}/work-items/{number}", null);
    }

    public sealed record Request
    {
        public required string Title { get; init; }
    }
}