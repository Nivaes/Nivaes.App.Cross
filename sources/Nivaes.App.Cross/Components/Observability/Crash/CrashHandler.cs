using System.Diagnostics;
using System.Text;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Logs;

namespace Nivaes.App.Cross.Observability;

public abstract class CrashHandler : ICrashHandler
{
    protected abstract string PathCrashFile { get; }

    protected readonly ILogger Logger;

    protected readonly LoggerProvider? LoggerProvider;

    public CrashHandler(ILogger logger, LoggerProvider? loggerProvider)
    {
        Logger = logger;
        LoggerProvider = loggerProvider;
    }

    public virtual void Register()
    {
        Task.Run(async () => LoadAndSendException());

        // Excepciones en código .NET
        AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

        // Excepciones en tareas asíncronas no observadas
        TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
    }

    protected virtual void SaveException(Exception ex, string description)
    {
        try
        {
            var message = Serialize(ex);
            File.WriteAllText(PathCrashFile, message);
        }
        catch { }
    }

    protected virtual async Task LoadAndSendException()
    {
        if (File.Exists(PathCrashFile))
        {
            var message = await File.ReadAllTextAsync(PathCrashFile);

            Logger.LogCritical(message);
            LoggerProvider?.ForceFlush();
        }
    }

    private void CurrentDomain_UnhandledException(
        object sender,
        UnhandledExceptionEventArgs e)
    {
        var ex = (Exception)e.ExceptionObject;

        SaveException(ex, "Unhandled exception occurred.");

        Logger.LogCritical(ex, "Unhandled exception occurred.");
    }

    private void TaskScheduler_UnobservedTaskException(
        object? sender,
        UnobservedTaskExceptionEventArgs e)
    {
        var ex = e.Exception;
        SaveException(ex, "Unobserved task exception occurred.");

        Logger.LogCritical(ex, "Unobserved task exception occurred.");

        e.SetObserved();
    }

    protected static string Serialize(Exception ex)
    {
        var crash = new CrashInfo(ex);
        return Serialize(crash);
    }

    private static string Serialize(CrashInfo crash)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"Timestamp: {crash.Timestamp}");
        sb.AppendLine($"Type: {crash.Type}");
        sb.AppendLine($"Message: {crash.Message}");
        sb.AppendLine($"StackTrace: {crash.StackTrace}");

        if (crash.Data != null && crash.Data.Count > 0)
            sb.AppendLine($"Data: {crash.Data}");

        if (crash.InnerException != null)
        {
            sb.AppendLine("InnerException:");
            sb.AppendLine(Serialize(crash.InnerException));
        }

        return sb.ToString();
    }
}
