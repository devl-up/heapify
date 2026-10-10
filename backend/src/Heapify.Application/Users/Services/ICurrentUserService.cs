using Heapify.Application.Users.DTO;

namespace Heapify.Application.Users.Services;

public interface ICurrentUserService
{
    Task<CurrentUserDto> GetAsync(CancellationToken cancellationToken = default);
}