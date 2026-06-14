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
