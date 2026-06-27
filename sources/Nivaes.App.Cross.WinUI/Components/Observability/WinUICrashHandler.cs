using Microsoft.Extensions.Logging;
using Nivaes.App.Cross.Observability;
using OpenTelemetry.Logs;

namespace Nivaes.App.Cross.WinUI.Observability
{
    public class WinUICrashHandler : CrashHandler
    {
        protected override string PathCrashFile => Path.Combine(
            Windows.Storage.ApplicationData.Current.LocalFolder.Path,
            "crash.log");

        public WinUICrashHandler(ILogger<WinUICrashHandler> logger, LoggerProvider? loggerProvider = null)
            : base(logger, loggerProvider)
        {
        }
    }
}
