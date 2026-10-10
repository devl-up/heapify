using System.Reflection;
using FluentValidation;
using Heapify.Application.Projects.Services;
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
                .AddTransient<IProjectService, ProjectService>();
        }
    }
}