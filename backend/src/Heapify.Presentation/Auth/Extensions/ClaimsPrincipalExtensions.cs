using System.Security.Claims;

namespace Heapify.Presentation.Auth.Extensions;

internal static class ClaimsPrincipalExtensions
{
    extension(ClaimsPrincipal user)
    {
        public Guid? GetUserId()
        {
            var value = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            return string.IsNullOrWhiteSpace(value) || !Guid.TryParse(value, out var id)
                ? null
                : id;
        }
    }
}