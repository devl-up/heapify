using Heapify.Application.Common.DTO;
using Heapify.Application.Projects.DTO;

namespace Heapify.Application.Projects.Services;

public interface IProjectService
{
    Task<Guid> CreateAsync(CreateProjectDto dto, CancellationToken cancellationToken = default);
    Task<PaginatedResult<ProjectDto>> GetAllAsync(int index, int size, CancellationToken cancellationToken = default);
}