namespace Heapify.Application.Projects.Commands.ReserveNextNumber;

public sealed record ReserveNextNumberRequest
{
    public required Guid ProjectId { get; init; }
}