using Microsoft.Extensions.Logging;
using Nivaes.App.Cross.Observability;
using OpenTelemetry.Logs;

namespace Nivaes.App.Cross.PWA.Observability
{
    public class WebCrashHandler : CrashHandler
    {
        protected override string PathCrashFile => throw new NotImplementedException();

        public WebCrashHandler(ILogger<WebCrashHandler> logger, LoggerProvider? loggerProvider = null)
            : base(logger, loggerProvider)
        {
        }

        public override void Register()
        {
            base.Register();
        }

        protected override Task SaveException(Exception ex, string description)
        {
            throw new NotImplementedException();
        }

        protected override Task LoadAndSendException()
        {
            throw new NotImplementedException();
        }
    }
}
