using System.Reflection;
using Mapster;
using MapsterMapper;

namespace Api.Common.Mapping;

public static class DependencyInjection
{
    public static IServiceCollection AddMapping(this IServiceCollection services)
    {
        TypeAdapterConfig config = TypeAdapterConfig.GlobalSettings;
        config.Scan(Assembly.GetExecutingAssembly());

        services.AddSingleton(config);
        services.AddSingleton<IMapper, ServiceMapper>();

        return services;
    }
}
