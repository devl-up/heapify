using Heapify.Presentation.Common.Enums;
using Heapify.Presentation.Common.Filters;

namespace Heapify.Presentation.Common.Extensions;

internal static class EndpointConventionBuilderExtensions
{
    extension(IEndpointConventionBuilder builder)
    {
        public IEndpointConventionBuilder RequireProjectPermission(ProjectPermission projectPermission,
            string routeParameter = "projectId")
        {
            return builder.AddEndpointFilter(new PermissionFilter(projectPermission, routeParameter));
        }
    }
}