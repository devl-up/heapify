namespace Heapify.Application.WorkItems.Commands.CreateWorkItem;

public sealed record CreateWorkItemRequest
{
    public required Guid ProjectId { get; init; }
    public required string Title { get; init; }
}