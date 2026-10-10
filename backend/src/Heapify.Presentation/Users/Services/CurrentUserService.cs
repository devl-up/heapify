using Heapify.Application.Users.DTO;
using Heapify.Application.Users.Services;
using Heapify.Presentation.Auth.Extensions;

namespace Heapify.Presentation.Users.Services;

internal sealed class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    public async Task<CurrentUserDto> GetAsync(CancellationToken cancellationToken = default)
    {
        var userId = httpContextAccessor.HttpContext?.User.GetUserId();
        if (!userId.HasValue)
        {
            throw new InvalidOperationException("User is not authenticated.");
        }

        return new CurrentUserDto
        {
            Id = userId.Value
        };
    }
}