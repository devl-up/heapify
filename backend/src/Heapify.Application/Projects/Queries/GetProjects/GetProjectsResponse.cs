namespace Heapify.Application.Projects.Queries.GetProjects;

public sealed record GetProjectsResponse
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
}