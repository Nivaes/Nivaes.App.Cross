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

        protected override void SaveException(Exception ex)
        {
            throw new NotImplementedException();
        }
    }
}
