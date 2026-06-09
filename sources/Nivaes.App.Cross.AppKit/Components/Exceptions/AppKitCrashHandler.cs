using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross.AppKitOS
{
    public class AppKitCrashHandler : CrashHandler
    {
        public AppKitCrashHandler(ILogger logger)
            : base(logger)
        {
        }

        public override void Register()
        {
            base.Register();
        }
    }
}
