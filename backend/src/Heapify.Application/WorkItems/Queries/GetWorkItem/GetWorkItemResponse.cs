namespace Heapify.Application.WorkItems.Queries.GetWorkItem;

public sealed record GetWorkItemResponse
{
    public required long Number { get; init; }
    public required string Title { get; init; }
}