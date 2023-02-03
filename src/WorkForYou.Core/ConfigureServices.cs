using Microsoft.Extensions.DependencyInjection;

namespace WorkForYou.Core;

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();

        return services;
    }
}
