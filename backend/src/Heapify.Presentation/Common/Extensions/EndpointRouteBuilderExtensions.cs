using System.Reflection;
using Heapify.Presentation.Common.Endpoints;

namespace Heapify.Presentation.Common.Extensions;

internal static class EndpointRouteBuilderExtensions
{
    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder builder)
    {
        var endpointTypes = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => typeof(IEndpoint).IsAssignableFrom(t) && t is { IsInterface: false, IsAbstract: false });

        foreach (var endpointType in endpointTypes)
        {
            var mapMethod = endpointType.GetMethod(nameof(IEndpoint.Map), BindingFlags.Public | BindingFlags.Static);

            if (mapMethod == null)
            {
                throw new InvalidOperationException(
                    $"The endpoint type '{endpointType.FullName}' does not have a static 'Map' method.");
            }

            mapMethod.Invoke(null, [builder]);
        }

        return builder;
    }
}