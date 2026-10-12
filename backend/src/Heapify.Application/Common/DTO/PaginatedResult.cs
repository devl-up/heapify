namespace Heapify.Application.Common.DTO;

public sealed class PaginatedResult<T>
{
    public required List<T> Items { get; init; }
    public required int TotalCount { get; init; }
}