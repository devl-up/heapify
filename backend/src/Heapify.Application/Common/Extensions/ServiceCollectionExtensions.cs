using Microsoft.Extensions.DependencyInjection;

namespace Heapify.Application.Common.Extensions;

public static class ServiceCollectionExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddApplicationLayer()
        {
            return services;
        }
    }
}