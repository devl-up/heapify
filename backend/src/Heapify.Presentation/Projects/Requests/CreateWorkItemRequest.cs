namespace Heapify.Presentation.Projects.Requests;

public sealed record CreateWorkItemRequest
{
    public required string Title { get; init; }
}