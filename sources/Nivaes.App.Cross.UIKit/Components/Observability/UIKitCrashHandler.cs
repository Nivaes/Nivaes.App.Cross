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
            ObjCRuntime.Runtime.MarshalManagedException += Runtime_MarshalManagedException;
        }

        protected override void SaveException(Exception ex)
        {
            throw new NotImplementedException();
        }

        protected override void LoadAndSendException(Exception ex)
        {
            throw new NotImplementedException();
        }

        private void Runtime_MarshalManagedException(object sender, ObjCRuntime.MarshalManagedExceptionEventArgs args)
        {
            var ex = args.Exception;

            SaveException(ex);

            base.Logger.LogCritical(ex, "Marshall managed exception ocurred");
        }
    }
}
