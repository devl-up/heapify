namespace Heapify.Application.WorkItems.DTO;

public sealed class WorkItemResponse
{
    public required long Number { get; init; }
    public required string Title { get; init; }
}