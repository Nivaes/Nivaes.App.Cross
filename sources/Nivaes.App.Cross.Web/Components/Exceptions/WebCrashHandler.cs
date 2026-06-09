using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross.Web
{
    public class WebCrashHandler : CrashHandler
    {
        public WebCrashHandler(ILogger logger)
            : base(logger)
        {
        }

        public override void Register()
        {
            base.Register();
        }
    }
}
