using Heapify.Presentation.Common.Enums;
using Heapify.Presentation.Common.Requirements;
using Microsoft.AspNetCore.Authorization;

namespace Heapify.Presentation.Common.Filters;

internal sealed class PermissionFilter(ProjectPermission projectPermission, string routeParameterName) : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var http = context.HttpContext;

        if (!http.Request.RouteValues.TryGetValue(routeParameterName, out var raw) ||
            !Guid.TryParse(raw?.ToString(), out var projectId))
        {
            return Results.Problem();
        }

        var authorization = http.RequestServices.GetRequiredService<IAuthorizationService>();

        var result = await authorization.AuthorizeAsync(
            http.User,
            projectId,
            new PermissionRequirement(projectPermission));

        if (!result.Succeeded)
        {
            return Results.Forbid();
        }

        return await next(context);
    }
}