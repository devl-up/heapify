using System.Net;
using Heapify.Application.Projects.Commands;
using Heapify.Presentation.Common.Endpoints;

namespace Heapify.Presentation.Projects.Endpoints;

internal sealed class CreateProjectEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder builder)
    {
        builder.MapPost("projects", HandleAsync)
            .Produces((int)HttpStatusCode.Created)
            .WithTags("Projects");
    }

    private static async Task<IResult> HandleAsync(
        CreateProject.Handler handler,
        CreateProject.Request request,
        CancellationToken cancellationToken)
    {
        var id = await handler.HandleAsync(request, cancellationToken);
        return Results.Created($"/api/v1/projects/{id}", null);
    }
}