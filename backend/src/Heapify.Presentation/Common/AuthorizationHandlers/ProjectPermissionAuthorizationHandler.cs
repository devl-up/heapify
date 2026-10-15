using Heapify.Domain.Projects.Entities;
using Heapify.Domain.Projects.Enums;
using Heapify.Infrastructure.Common.Data;
using Heapify.Presentation.Auth.Extensions;
using Heapify.Presentation.Common.Enums;
using Heapify.Presentation.Common.Requirements;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Heapify.Presentation.Common.AuthorizationHandlers;

internal sealed class ProjectPermissionAuthorizationHandler(ApplicationDbContext dbContext)
    : AuthorizationHandler<PermissionRequirement, Guid>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionRequirement requirement,
        Guid resource
    )
    {
        var userId = context.User.GetUserId();
        if (userId is null)
        {
            context.Fail(new AuthorizationFailureReason(this, "User is not authenticated."));
            return;
        }

        var member = await dbContext.Set<Member>()
            .Where(m => m.ProjectId == resource && m.UserId == userId)
            .Select(m => new { m.Role })
            .FirstOrDefaultAsync();

        if (member is null)
        {
            context.Fail(new AuthorizationFailureReason(this, "User is not a member of the project."));
            return;
        }

        if (!HasPermission(member.Role, requirement.ProjectPermission))
        {
            context.Fail(new AuthorizationFailureReason(this, "User does not have the required permission."));
            return;
        }

        context.Succeed(requirement);
    }

    private static bool HasPermission(MemberRole role, ProjectPermission projectPermission)
    {
        return role switch
        {
            MemberRole.Admin => true,
            MemberRole.Member => projectPermission is ProjectPermission.Read or ProjectPermission.Write,
            MemberRole.Viewer => projectPermission == ProjectPermission.Read,
            _ => false
        };
    }
}