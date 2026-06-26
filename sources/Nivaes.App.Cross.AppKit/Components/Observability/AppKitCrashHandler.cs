using Microsoft.Extensions.Logging;
using Nivaes.App.Cross.Observability;
using OpenTelemetry.Logs;

namespace Nivaes.App.Cross.AppKitOS.Observability
{
    public class AppKitCrashHandler : CrashHandler
    {
        protected override string PathCrashFile => Path.Combine(
              Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
             "crash.log");

        public AppKitCrashHandler(ILogger<AppKitCrashHandler> logger, LoggerProvider loggerProvider)
            : base(logger, loggerProvider)
        {
        }

        public override void Register()
        {
            base.Register();

            ObjCRuntime.Runtime.MarshalManagedException += Runtime_MarshalManagedException;
        }

        private void Runtime_MarshalManagedException(object sender, ObjCRuntime.MarshalManagedExceptionEventArgs args)
        {
            var ex = args.Exception;

            SaveException(ex, "Marshall managed exception ocurred");

            base.Logger.LogCritical(ex, "Marshall managed exception ocurred");
            LoggerProvider.ForceFlush();
        }
    }
}
