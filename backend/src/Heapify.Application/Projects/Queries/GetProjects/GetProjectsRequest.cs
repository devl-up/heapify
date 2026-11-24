namespace Heapify.Application.Projects.Queries.GetProjects;

public sealed record GetProjectsRequest
{
    public required int Index { get; init; }
    public required int Size { get; init; }
}