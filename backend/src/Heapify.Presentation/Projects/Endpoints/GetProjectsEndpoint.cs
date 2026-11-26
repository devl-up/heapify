using Heapify.Application.Projects.Queries;
using Heapify.Presentation.Common.Endpoints;
using Microsoft.AspNetCore.Mvc;

namespace Heapify.Presentation.Projects.Endpoints;

internal sealed class GetProjectsEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder builder)
    {
        builder.MapGet("projects", HandleAsync)
            .Produces<GetProjects.Response>()
            .WithTags("Projects");
    }

    private static async Task<IResult> HandleAsync(
        GetProjects.Handler handler,
        [FromQuery] int index,
        [FromQuery] int size,
        CancellationToken cancellationToken)
    {
        var request = new GetProjects.Request
        {
            Index = index,
            Size = size
        };

        var response = await handler.HandleAsync(request, cancellationToken);
        return Results.Ok(response);
    }
}