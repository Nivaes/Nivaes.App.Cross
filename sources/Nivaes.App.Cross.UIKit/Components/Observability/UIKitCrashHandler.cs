using Microsoft.Extensions.Logging;
using Nivaes.App.Cross.Observability;
using OpenTelemetry.Logs;

namespace Nivaes.App.Cross.UIKitOS.Observability
{
    public class UIKitCrashHandler : CrashHandler
    {
        protected override string PathCrashFile => Path.Combine(
                      Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                     "crash.log");

        public UIKitCrashHandler(ILogger<UIKitCrashHandler> logger, LoggerProvider loggerProvider)
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
