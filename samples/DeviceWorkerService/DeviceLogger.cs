using Microsoft.Extensions.Logging;
using Secs4Net;
using Secs4Net.Sml;
using System;

namespace DeviceWorkerService;

internal sealed class DeviceLogger(ILogger<DeviceLogger> logger) : ISecsGemLogger
{
    public void MessageIn(SecsMessage msg, int id) => logger.LogTrace($"<-- [0x{id:X8}] {msg.ToSml()}");
    public void MessageOut(SecsMessage msg, int id) => logger.LogTrace($"--> [0x{id:X8}] {msg.ToSml()}");
    public void Debug(string msg) => logger.LogDebug(msg);
    public void Info(string msg) => logger.LogInformation(msg);
    public void Warning(string msg) => logger.LogWarning(msg);
    public void Error(string msg, SecsMessage? message, Exception? ex) => logger.LogError(ex, $"{msg} {message}\n");
}
