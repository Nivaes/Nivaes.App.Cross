using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross.UIKitOS
{
    public class UIKitCrashHandler : CrashHandler
    {
        public UIKitCrashHandler(ILogger<UIKitCrashHandler> logger)
            : base(logger)
        {
        }

        public override void Register()
        {
            base.Register();
        }
    }
}
