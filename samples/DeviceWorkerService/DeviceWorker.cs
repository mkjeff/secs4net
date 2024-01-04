using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Secs4Net;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DeviceWorkerService;

internal sealed class DeviceWorker : BackgroundService
{
    private readonly ILogger<DeviceWorker> _logger;
    private readonly ISecsConnection _hsmsConnection;
    private readonly ISecsGem _secsGem;

    public DeviceWorker(ILogger<DeviceWorker> logger, ISecsConnection hsmsConnection, ISecsGem secsGem)
    {
        _logger = logger;
        _hsmsConnection = hsmsConnection;
        _secsGem = secsGem;

        _hsmsConnection.ConnectionChanged += delegate
         {
             switch (_hsmsConnection.State)
             {
                 case ConnectionState.Retry:
                     _logger.LogError($"Connection loss, try to reconnect.");
                     break;
                 case ConnectionState.Connecting:
                 case ConnectionState.Connected:
                     _logger.LogWarning(_hsmsConnection.State.ToString());
                     break;
                 default:
                     _logger.LogInformation($"Connection state = {_hsmsConnection.State}");
                     break;
             }
         };
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            _hsmsConnection.Start(stoppingToken);
            await foreach (var e in _secsGem.GetPrimaryMessageAsync(stoppingToken))
            {
                using var primaryMessage = e.PrimaryMessage;
                _logger.LogInformation($"Received primary message: {primaryMessage}");
                try
                {
                    using var secondaryMessage = new SecsMessage(primaryMessage.S, (byte)(primaryMessage.F + 1))
                    {
                        SecsItem = primaryMessage.SecsItem,
                    };
                    await e.TryReplyAsync(secondaryMessage, stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Exception occurred when processing primary message");
                }
            }
        }
        catch (Exception ex)
        {
            if (stoppingToken.IsCancellationRequested)
            {
                return;
            }
            _logger.LogError(ex, "Unhandled exception occurred on primary messages processing");
        }
    }
}
