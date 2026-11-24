namespace Heapify.Application.WorkItems.Queries.GetWorkItems;

public sealed record GetWorkItemsRequest
{
    public required int Index { get; init; }
    public required int Size { get; init; }
    public required Guid ProjectId { get; init; }
}