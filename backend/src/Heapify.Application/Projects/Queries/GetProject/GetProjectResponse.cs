namespace Heapify.Application.Projects.Queries.GetProject;

public sealed record GetProjectResponse
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
}