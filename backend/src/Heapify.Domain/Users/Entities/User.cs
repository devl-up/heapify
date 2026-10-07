using Microsoft.AspNetCore.Identity;

namespace Heapify.Domain.Users.Entities;

public sealed class User : IdentityUser<Guid>;