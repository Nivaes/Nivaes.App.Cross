using Microsoft.Extensions.Logging;
using Nivaes.App.Cross.Observability;
using OpenTelemetry.Logs;

namespace Nivaes.App.Cross.Web.Observability
{
    public class WebCrashHandler : CrashHandler
    {
        protected override string PathCrashFile => throw new NotImplementedException();

        public WebCrashHandler(ILogger<WebCrashHandler> logger, LoggerProvider loggerProvider)
            : base(logger, loggerProvider)
        {
        }

        public override void Register()
        {
            base.Register();
        }

        protected override void SaveException(Exception ex, string description)
        {
            throw new NotImplementedException();
        }

        protected override Task LoadAndSendException()
        {
            throw new NotImplementedException();
        }
    }
}
