using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross.WinUI
{
    public class WinUICrashHandler : CrashHandler
    {
        public WinUICrashHandler(ILogger<WinUICrashHandler> logger)
            : base(logger)
        {
        }

        public override void Register()
        {
            base.Register();
        }
    }
}
