using Microsoft.Extensions.Logging;
using Secs4Net;
using Secs4Net.Sml;
using System;

namespace DeviceWorkerService
{
    internal sealed class DeviceLogger : ISecsGemLogger
    {
        private readonly ILogger<DeviceLogger> _logger;

        public DeviceLogger(ILogger<DeviceLogger> logger)
        {
            _logger = logger;
        }

        public void MessageIn(SecsMessage msg, int systembyte) => _logger.LogTrace($"<-- [0x{systembyte:X8}] {msg.ToSml()}");
        public void MessageOut(SecsMessage msg, int systembyte) => _logger.LogTrace($"--> [0x{systembyte:X8}] {msg.ToSml()}");
        public void Debug(string msg) => _logger.LogDebug(msg);
        public void Info(string msg) => _logger.LogInformation(msg);
        public void Warning(string msg) => _logger.LogWarning(msg);
        public void Error(string msg, SecsMessage? message, Exception? ex) => _logger.LogError(ex, $"{msg} {message}\n");
    }
}
