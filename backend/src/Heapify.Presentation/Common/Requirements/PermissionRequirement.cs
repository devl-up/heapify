using Heapify.Presentation.Common.Enums;
using Microsoft.AspNetCore.Authorization;

namespace Heapify.Presentation.Common.Requirements;

internal sealed class PermissionRequirement(ProjectPermission projectPermission) : IAuthorizationRequirement
{
    public ProjectPermission ProjectPermission { get; } = projectPermission;
}