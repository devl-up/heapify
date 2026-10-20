using Heapify.Application.WorkItems.DTO;

namespace Heapify.Application.WorkItems.Services;

public interface IWorkItemService
{
    Task<long> CreateAsync(CreateWorkItemRequest request, CancellationToken cancellationToken);
}