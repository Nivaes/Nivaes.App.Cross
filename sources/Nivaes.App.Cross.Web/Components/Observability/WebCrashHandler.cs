using Microsoft.Extensions.Logging;
using Nivaes.App.Cross.Components.Exceptions.Crash;

namespace Nivaes.App.Cross.Web
{
    public class WebCrashHandler : CrashHandler
    {
        public WebCrashHandler(ILogger<WebCrashHandler> logger)
            : base(logger)
        {
        }

        public override void Register()
        {
            base.Register();
        }

        protected override void Report(Exception ex)
        {
            throw new NotImplementedException();
        }
    }
}
