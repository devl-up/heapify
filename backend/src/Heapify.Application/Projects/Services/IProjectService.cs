using Heapify.Application.Common.DTO;
using Heapify.Application.Projects.DTO;

namespace Heapify.Application.Projects.Services;

public interface IProjectService
{
    Task<Guid> CreateAsync(CreateProjectRequest request, CancellationToken cancellationToken);
    Task<PaginatedResponse<ProjectResponse>> GetAllAsync(GetProjectsRequest request, CancellationToken cancellationToken);
}