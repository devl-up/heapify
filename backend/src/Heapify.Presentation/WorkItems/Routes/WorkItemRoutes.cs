using System.Security.Claims;
using Heapify.Application.WorkItems.DTO;
using Heapify.Application.WorkItems.Services;
using Heapify.Presentation.Common.Enums;
using Heapify.Presentation.Common.Requirements;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Heapify.Presentation.WorkItems.Routes;

internal static class WorkItemRoutes
{
    extension(IEndpointRouteBuilder endpoints)
    {
        public IEndpointRouteBuilder MapWorkItemRoutes()
        {
            var group = endpoints.MapGroup("work-items")
                .WithTags("Work Items");

            group.MapPost("",
                async (ClaimsPrincipal user, IAuthorizationService authorizationService, IWorkItemService service,
                    [FromBody] CreateWorkItemRequest request, CancellationToken cancellationToken) =>
                {
                    var authorizationResult = await authorizationService.AuthorizeAsync(user, request.ProjectId,
                        new PermissionRequirement(ProjectPermission.Write));

                    if (!authorizationResult.Succeeded)
                    {
                        return Results.Forbid();
                    }

                    var number = await service.CreateAsync(request, cancellationToken);
                    return Results.Created($"/api/v1/projects/{request.ProjectId}/work-items/{number}", null);
                });

            return endpoints;
        }
    }
}