using Heapify.Domain.Projects.Enums;

namespace Heapify.Domain.Projects.Entities;

public sealed class Member
{
    public required Guid ProjectId { get; init; }
    public required Guid UserId { get; init; }
    public required MemberRole Role { get; init; }
}