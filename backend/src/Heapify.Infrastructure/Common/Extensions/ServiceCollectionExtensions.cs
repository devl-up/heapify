using Heapify.Application.Common.Data;
using Heapify.Infrastructure.Common.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Heapify.Infrastructure.Common.Extensions;

public static class ServiceCollectionExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddInfrastructureLayer(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Postgres");
            ArgumentNullException.ThrowIfNull(connectionString);

            services.AddDbContext<ApplicationDbContext>(builder =>
                builder.UseNpgsql(connectionString)
                    .UseSnakeCaseNamingConvention());

            services.AddTransient<IApplicationDbContext, ApplicationDbContext>();

            return services;
        }
    }
}