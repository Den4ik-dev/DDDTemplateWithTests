using Infrastructure.Data;
using Infrastructure.Data.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventInterceptor>();

        string connectionString = configuration.GetConnectionString("Default")!;
        services.AddDbContext<ApplicationDbContext>(
            (serviceProvider, options) =>
            {
                options.AddInterceptors(serviceProvider.GetServices<ISaveChangesInterceptor>());

                options.UseSqlServer(connectionString);
            }
        );

        return services;
    }
}
