using Heapify.Application.Projects.DTO;
using Heapify.Application.Projects.Services;
using Heapify.Presentation.Common.Enums;
using Heapify.Presentation.Common.Extensions;
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
                async ([FromServices] IProjectService service, [AsParameters] GetProjectsRequest request,
                    CancellationToken cancellationToken) =>
                {
                    var response = await service.GetAllAsync(request, cancellationToken);
                    return Results.Ok(response);
                });

            group.MapGet("{projectId:guid}", async ([FromServices] IProjectService service, [FromRoute] Guid projectId,
                    CancellationToken cancellationToken) =>
                {
                    var response = await service.GetAsync(projectId, cancellationToken);
                    return Results.Ok(response);
                })
                .RequireProjectPermission(ProjectPermission.Read);

            group.MapPost("",
                async ([FromServices] IProjectService service, [FromBody] CreateProjectRequest request,
                    CancellationToken cancellationToken) =>
                {
                    var id = await service.CreateAsync(request, cancellationToken);
                    return Results.Created($"/api/v1/projects/{id}", null);
                });

            return endpoints;
        }
    }
}