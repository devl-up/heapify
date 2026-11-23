using Heapify.Application.Common.DTO;
using Heapify.Application.WorkItems.DTO;

namespace Heapify.Application.WorkItems.Services;

public interface IWorkItemService
{
    Task<long> CreateAsync(Guid projectId, CreateWorkItemRequest request, CancellationToken cancellationToken);
    Task<PaginatedResponse<WorkItemResponse>> GetAllAsync(GetWorkItemsRequest request, CancellationToken cancellationToken);
    Task<WorkItemResponse> GetAsync(Guid projectId, long number, CancellationToken cancellationToken);
}