using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Secs4Net;

public static class ServiceProvider
{
    public static IServiceCollection AddSecs4Net<TLogger>(this IServiceCollection services, IConfiguration configuration)
        where TLogger : class, ISecsGemLogger
    {
        services.Configure<SecsGemOptions>(configuration.GetSection("secs4net"));
        services.AddSingleton<ISecsConnection, HsmsConnection>();
        services.AddSingleton<ISecsGem, SecsGem>();
        services.AddSingleton<ISecsGemLogger, TLogger>();
        services.AddHostedService(static s => (HsmsConnection)s.GetRequiredService<ISecsConnection>());
        return services;
    }
}
