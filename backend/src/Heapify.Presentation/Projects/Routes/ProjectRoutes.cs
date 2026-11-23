using System.Security.Claims;
using Heapify.Application.Projects.DTO;
using Heapify.Application.Projects.Services;
using Heapify.Application.WorkItems.DTO;
using Heapify.Application.WorkItems.Services;
using Heapify.Presentation.Common.Enums;
using Heapify.Presentation.Common.Requirements;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Heapify.Presentation.Projects.Routes;

internal static class ProjectRoutes
{
    extension(IEndpointRouteBuilder endpoints)
    {
        public void MapProjectRoutes()
        {
            var projectsEndpoints = endpoints.MapGroup("projects")
                .WithTags("Projects");

            projectsEndpoints.MapGet("",
                async (IProjectService service, [FromQuery] int index, [FromQuery] int size,
                    CancellationToken cancellationToken) =>
                {
                    var request = new GetProjectsRequest
                    {
                        Index = index,
                        Size = size
                    };

                    var response = await service.GetAllAsync(request, cancellationToken);
                    return Results.Ok(response);
                });

            projectsEndpoints.MapPost("",
                async (IProjectService service, CreateProjectRequest request, CancellationToken cancellationToken) =>
                {
                    var id = await service.CreateAsync(request, cancellationToken);
                    return Results.Created($"/api/v1/projects/{id}", null);
                });

            var projectDetailEndpoints = projectsEndpoints.MapGroup("{id:guid}");

            projectDetailEndpoints.MapGet("", async (ClaimsPrincipal user, IAuthorizationService authorizationService,
                IProjectService service, [FromRoute] Guid id, CancellationToken cancellationToken) =>
            {
                var authorizationResult =
                    await authorizationService.AuthorizeAsync(user, id,
                        new PermissionRequirement(ProjectPermission.Read));

                if (!authorizationResult.Succeeded)
                {
                    return Results.Forbid();
                }

                var response = await service.GetAsync(id, cancellationToken);
                return Results.Ok(response);
            });

            projectDetailEndpoints.MapProjectWorkItemRoutes();
        }

        private void MapProjectWorkItemRoutes()
        {
            var projectWorkItemsEndpoints = endpoints.MapGroup("work-items");

            projectWorkItemsEndpoints.MapGet("",
                async (ClaimsPrincipal user, IAuthorizationService authorizationService, IWorkItemService service,
                    [FromRoute] Guid id, [FromQuery] int index, [FromQuery] int size,
                    CancellationToken cancellationToken) =>
                {
                    var authorizationResult =
                        await authorizationService.AuthorizeAsync(user, id,
                            new PermissionRequirement(ProjectPermission.Read));

                    if (!authorizationResult.Succeeded)
                    {
                        return Results.Forbid();
                    }

                    var request = new GetWorkItemsRequest
                    {
                        ProjectId = id,
                        Index = index,
                        Size = size
                    };

                    var response = await service.GetAllAsync(request, cancellationToken);
                    return Results.Ok(response);
                });

            projectWorkItemsEndpoints.MapGet("{number:long}",
                async (ClaimsPrincipal user, IAuthorizationService authorizationService, IWorkItemService service,
                    [FromRoute] Guid id, [FromRoute] long number,
                    CancellationToken cancellationToken) =>
                {
                    var authorizationResult =
                        await authorizationService.AuthorizeAsync(user, id,
                            new PermissionRequirement(ProjectPermission.Read));

                    if (!authorizationResult.Succeeded)
                    {
                        return Results.Forbid();
                    }

                    var response = await service.GetAsync(id, number, cancellationToken);
                    return Results.Ok(response);
                });

            projectWorkItemsEndpoints.MapPost("",
                async (ClaimsPrincipal user, IAuthorizationService authorizationService, IWorkItemService service,
                    Guid id, [FromBody] CreateWorkItemRequest request, CancellationToken cancellationToken) =>
                {
                    var authorizationResult = await authorizationService.AuthorizeAsync(user, id,
                        new PermissionRequirement(ProjectPermission.Write));

                    if (!authorizationResult.Succeeded)
                    {
                        return Results.Forbid();
                    }

                    var number = await service.CreateAsync(id, request, cancellationToken);
                    return Results.Created($"/api/v1/projects/{id}/work-items/{number}", null);
                });
        }
    }
}