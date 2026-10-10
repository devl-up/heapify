namespace Heapify.Domain.Projects.Entities;

public sealed class Project
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
}