using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Secs4Net;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DeviceWorkerService
{
    internal sealed class DeviceWorker : BackgroundService
    {
        private readonly ILogger<DeviceWorker> _logger;
        private readonly IHsmsConnection _hsmsConnection;
        private readonly ISecsGem _secsGem;

        public DeviceWorker(ILogger<DeviceWorker> logger, IHsmsConnection hsmsConnection, ISecsGem secsGem)
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
                await foreach (var m in _secsGem.GetPrimaryMessageAsync(stoppingToken))
                {
                    _logger.LogInformation($"Received primary message: {m.PrimaryMessage.ToString()}");
                    await m.TryReplyAsync(new SecsMessage(m.PrimaryMessage.S, (byte)(m.PrimaryMessage.F + 1))
                    {
                        SecsItem = m.PrimaryMessage.SecsItem,
                    }, stoppingToken);
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
}
