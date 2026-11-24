namespace Heapify.Application.WorkItems.Queries.GetWorkItem;

public sealed record GetWorkItemRequest
{
    public required Guid ProjectId { get; init; }
    public required long Number { get; init; }
}