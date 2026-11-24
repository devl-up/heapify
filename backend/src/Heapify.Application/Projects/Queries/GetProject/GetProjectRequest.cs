namespace Heapify.Application.Projects.Queries.GetProject;

public sealed record GetProjectRequest
{
    public required Guid Id { get; init; }
}