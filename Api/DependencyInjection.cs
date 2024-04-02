using Api.Common.Mapping;
using Api.Infrastructure;

namespace Api;

public static class DependencyInjection
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        services.AddAuthentication();
        
        services.AddMapping();
        services.AddControllers();
        services.AddSwaggerGen();
        services.AddExceptionHandler<CustomExceptionHandler>();

        return services;
    }
}
