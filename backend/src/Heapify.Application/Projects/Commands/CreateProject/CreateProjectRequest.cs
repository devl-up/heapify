namespace Heapify.Application.Projects.Commands.CreateProject;

public sealed record  CreateProjectRequest
{
    public required string Name { get; init; }
}