using Heapify.Application.Users.DTO;

namespace Heapify.Application.Users.Services;

public interface ICurrentUserService
{
    CurrentUser Get();
}