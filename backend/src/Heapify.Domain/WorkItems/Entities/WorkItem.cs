namespace Heapify.Domain.WorkItems.Entities;

public sealed class WorkItem
{
    public required Guid ProjectId { get; init; }
    public required long Number { get; init; }
    public required string Title { get; init; }
}