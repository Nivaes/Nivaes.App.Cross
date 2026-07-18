using Microsoft.Extensions.Logging;
using Nivaes.App.Cross.Observability;
using OpenTelemetry.Logs;

namespace Nivaes.App.Cross.AppKitLib.Observability
{
    internal class AppKitCrashHandler : CrashHandler
    {
        protected override string PathCrashFile => Path.Combine(
              Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
             "crash.log");

        public AppKitCrashHandler(ILogger<AppKitCrashHandler> logger, LoggerProvider? loggerProvider = null)
            : base(logger, loggerProvider)
        {
        }

        public override void Register()
        {
            base.Register();

            ObjCRuntime.Runtime.MarshalManagedException += Runtime_MarshalManagedException;
            ObjCRuntime.Runtime.MarshalObjectiveCException += Runtime_MarshalObjectiveCException;
        }

        private async void Runtime_MarshalManagedException(object sender, ObjCRuntime.MarshalManagedExceptionEventArgs args)
        {
            var ex = args.Exception;

            await SaveException(ex, "Marshall managed exception ocurred");

            base.Logger.LogCritical(ex, "Marshall managed exception ocurred");
            LoggerProvider?.ForceFlush();
        }

        private async void Runtime_MarshalObjectiveCException(object sender, ObjCRuntime.MarshalObjectiveCExceptionEventArgs args)
        {
            var ex = new NSExceptionWrapper(args.Exception);

            await SaveException(ex, "MarshallC managed exception ocurred");

            base.Logger.LogCritical(ex, "Marshall managed exception ocurred");
            LoggerProvider?.ForceFlush();
        }

    }
}
