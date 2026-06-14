using Microsoft.Extensions.Logging;

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
