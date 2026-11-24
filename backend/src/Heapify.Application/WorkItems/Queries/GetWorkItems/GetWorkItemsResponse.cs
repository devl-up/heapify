namespace Heapify.Application.WorkItems.Queries.GetWorkItems;

public sealed record GetWorkItemsResponse
{
    public required long Number { get; init; }
    public required string Title { get; init; }
}