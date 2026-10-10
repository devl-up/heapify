using Heapify.Application.Projects.DTO;
using Heapify.Application.Projects.Services;
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

            group.MapPost("",
                async ([FromServices] IProjectService service, [FromBody] CreateProjectDto dto,
                    CancellationToken cancellationToken = default) =>
                {
                    var id = await service.CreateAsync(dto, cancellationToken);
                    return Results.Created($"/api/v1/projects/{id}", null);
                });

            return endpoints;
        }
    }
}