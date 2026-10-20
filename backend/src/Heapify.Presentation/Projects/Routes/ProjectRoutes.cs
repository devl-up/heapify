using System.Security.Claims;
using Heapify.Application.Projects.DTO;
using Heapify.Application.Projects.Services;
using Heapify.Presentation.Common.Enums;
using Heapify.Presentation.Common.Requirements;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Heapify.Presentation.Projects.Routes;

internal static class ProjectRoutes
{
    extension(IEndpointRouteBuilder endpoints)
    {
        public IEndpointRouteBuilder MapProjectRoutes()
        {
            var group = endpoints.MapGroup("projects")
                .WithTags("Projects");

            group.MapGet("",
                async (IProjectService service, [AsParameters] GetProjectsRequest request,
                    CancellationToken cancellationToken) =>
                {
                    var response = await service.GetAllAsync(request, cancellationToken);
                    return Results.Ok(response);
                });

            group.MapGet("{id:guid}", async (ClaimsPrincipal user, IAuthorizationService authorizationService,
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

            group.MapPost("",
                async (IProjectService service, CreateProjectRequest request, CancellationToken cancellationToken) =>
                {
                    var id = await service.CreateAsync(request, cancellationToken);
                    return Results.Created($"/api/v1/projects/{id}", null);
                });

            return endpoints;
        }
    }
}