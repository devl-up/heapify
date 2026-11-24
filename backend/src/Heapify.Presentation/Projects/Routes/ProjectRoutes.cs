using System.Security.Claims;
using Heapify.Application.Projects.Commands.CreateProject;
using Heapify.Application.Projects.Queries.GetProject;
using Heapify.Application.Projects.Queries.GetProjects;
using Heapify.Application.WorkItems.Commands.CreateWorkItem;
using Heapify.Application.WorkItems.Queries.GetWorkItem;
using Heapify.Application.WorkItems.Queries.GetWorkItems;
using Heapify.Presentation.Common.Enums;
using Heapify.Presentation.Common.Requirements;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CreateWorkItemRequest = Heapify.Application.WorkItems.Commands.CreateWorkItem.CreateWorkItemRequest;

namespace Heapify.Presentation.Projects.Routes;

internal static class ProjectRoutes
{
    extension(IEndpointRouteBuilder endpoints)
    {
        public void MapProjectRoutes()
        {
            var group = endpoints.MapGroup("projects")
                .WithTags("Projects");

            group.MapGet("",
                async (GetProjectsHandler handler, [FromQuery] int index, [FromQuery] int size,
                    CancellationToken cancellationToken) =>
                {
                    var request = new GetProjectsRequest
                    {
                        Index = index,
                        Size = size
                    };

                    var response = await handler.HandleAsync(request, cancellationToken);
                    return Results.Ok(response);
                });

            group.MapPost("", async (CreateProjectHandler handler, CreateProjectRequest request,
                CancellationToken cancellationToken) =>
            {
                var id = await handler.HandleAsync(request, cancellationToken);
                return Results.Created($"/api/v1/projects/{id}", null);
            });


            group.MapGet("{id:guid}", async (ClaimsPrincipal user, IAuthorizationService authorizationService,
                GetProjectHandler handler, [FromRoute] Guid id, CancellationToken cancellationToken) =>
            {
                var authorizationResult =
                    await authorizationService.AuthorizeAsync(user, id,
                        new PermissionRequirement(ProjectPermission.Read));

                if (!authorizationResult.Succeeded)
                {
                    return Results.Forbid();
                }

                var request = new GetProjectRequest
                {
                    Id = id
                };

                var response = await handler.HandleAsync(request, cancellationToken);

                return Results.Ok(response);
            });

            group.MapGet("{id:guid}/work-items",
                async (ClaimsPrincipal user, IAuthorizationService authorizationService, GetWorkItemsHandler handler,
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

                    var response = await handler.HandleAsync(request, cancellationToken);
                    return Results.Ok(response);
                });

            group.MapGet("{id:guid}/work-items/{number:long}",
                async (ClaimsPrincipal user, IAuthorizationService authorizationService, GetWorkItemHandler handler,
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

                    var request = new GetWorkItemRequest
                    {
                        ProjectId = id,
                        Number = number
                    };

                    var response = await handler.HandleAsync(request, cancellationToken);
                    return Results.Ok(response);
                });

            group.MapPost("{id:guid}/work-items",
                async (ClaimsPrincipal user, IAuthorizationService authorizationService, CreateWorkItemHandler handler,
                    Guid id, [FromBody] Requests.CreateWorkItemRequest request, CancellationToken cancellationToken) =>
                {
                    var authorizationResult = await authorizationService.AuthorizeAsync(user, id,
                        new PermissionRequirement(ProjectPermission.Write));

                    if (!authorizationResult.Succeeded)
                    {
                        return Results.Forbid();
                    }

                    var applicationRequest = new CreateWorkItemRequest
                    {
                        ProjectId = id,
                        Title = request.Title
                    };

                    var number = await handler.HandleAsync(applicationRequest, cancellationToken);
                    return Results.Created($"/api/v1/projects/{id}/work-items/{number}", null);
                });
        }
    }
}