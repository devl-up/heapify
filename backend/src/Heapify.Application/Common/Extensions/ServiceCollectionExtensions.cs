using System.Reflection;
using FluentValidation;
using Heapify.Application.Common.Requests;
using Microsoft.Extensions.DependencyInjection;

namespace Heapify.Application.Common.Extensions;

public static class ServiceCollectionExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddApplicationLayer()
        {
            return services
                .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly(), includeInternalTypes: true)
                .AddRequestHandlers();
        }

        private IServiceCollection AddRequestHandlers()
        {
            var requestHandlerInterfaceTypes = new[] { typeof(IRequestHandler<>), typeof(IRequestHandler<,>) };

            var requestHandlerTypes = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => t is { IsClass: true, IsAbstract: false })
                .SelectMany(t => t.GetInterfaces(), (t, i) => new { Type = t, Interface = i })
                .Where(x => x.Interface.IsGenericType &&
                            requestHandlerInterfaceTypes.Contains(x.Interface.GetGenericTypeDefinition()))
                .ToList();

            foreach (var handler in requestHandlerTypes)
            {
                services.AddTransient(handler.Type);
            }

            return services;
        }
    }
}