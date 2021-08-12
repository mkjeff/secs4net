using DeviceWorkerService;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Secs4Net;


Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddSecs4Net<DeviceLogger>(hostContext.Configuration);
        services.AddHostedService<DeviceWorker>();
    }).Build().Run();
