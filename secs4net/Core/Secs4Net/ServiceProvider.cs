using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Secs4Net
{
    public static class ServiceProvider
    {
        public static IServiceCollection AddSecs4Net<T>(this IServiceCollection services, IConfiguration configuration) where T : class, ISecsGemLogger
        {
            services.Configure<SecsGemOptions>(configuration.GetSection("secs4net"));
            services.AddSingleton<IHsmsConnection, HsmsConnection>();
            services.AddSingleton<ISecsGem, SecsGem>();
            services.AddSingleton<ISecsGemLogger, T>();
            services.AddHostedService(s => (HsmsConnection)s.GetRequiredService<IHsmsConnection>());
            return services;
        }
    }
}
