using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Secs4Net;
using System.Diagnostics.CodeAnalysis;

namespace DeviceWorkerService;

public static class ServiceProvider
{
    public static IServiceCollection AddSecs4Net<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TLogger>(this IServiceCollection services, IConfiguration configuration)
        where TLogger : class, ISecsGemLogger
    {
        var configSection = configuration.GetSection("secs4net");
        services.Configure<SecsGemOptions>(configSection);
        services.AddSingleton<ISecsConnection, HsmsConnection>();
        services.AddSingleton<ISecsGem, SecsGem>();
        services.AddSingleton<ISecsGemLogger, TLogger>();
        return services;
    }
}
