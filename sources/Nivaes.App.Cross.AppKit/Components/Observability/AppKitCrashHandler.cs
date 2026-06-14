using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross.AppKitOS
{
    public class AppKitCrashHandler : CrashHandler
    {
        public AppKitCrashHandler(ILogger<AppKitCrashHandler> logger)
            : base(logger)
        {
        }

        public override void Register()
        {
            base.Register();

            ObjCRuntime.Runtime.MarshalManagedException += Runtime_MarshalManagedException;
        }

        protected override void SaveException(Exception ex, string description)
        {
            throw new NotImplementedException();
        }

        protected override Task LoadAndSendException()
        {
            throw new NotImplementedException();
        }

        private void Runtime_MarshalManagedException(object sender, ObjCRuntime.MarshalManagedExceptionEventArgs args)
        {
            var ex = args.Exception;

            SaveException(ex, "Marshall managed exception ocurred");

            base.Logger.LogCritical(ex, "Marshall managed exception ocurred");
        }
    }
}
