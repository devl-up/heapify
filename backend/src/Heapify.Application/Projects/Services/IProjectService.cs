using Heapify.Application.Projects.DTO;

namespace Heapify.Application.Projects.Services;

public interface IProjectService
{
    Task<Guid> CreateAsync(CreateProjectDto dto, CancellationToken cancellationToken = default);
}