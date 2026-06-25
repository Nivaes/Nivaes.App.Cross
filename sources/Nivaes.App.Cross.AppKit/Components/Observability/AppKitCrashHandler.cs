using Microsoft.Extensions.Logging;
using OpenTelemetry.Logs;

namespace Nivaes.App.Cross.AppKitOS
{
    public class AppKitCrashHandler : CrashHandler
    {
        private string PathCrashFile => Path.Combine(
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

        protected override void SaveException(Exception ex, string description)
        {
            try
            {
                var message = Serialize(ex);
                File.WriteAllText(PathCrashFile, message);
            }
            catch { }
        }

        protected override async Task LoadAndSendException()
        {
            if (File.Exists(PathCrashFile))
            {
                var message = await File.ReadAllTextAsync(PathCrashFile);

                base.Logger.LogCritical(message);
                LoggerProvider.ForceFlush();

            }
        }

        private void Runtime_MarshalManagedException(object sender, ObjCRuntime.MarshalManagedExceptionEventArgs args)
        {
            var ex = args.Exception;

            SaveException(ex, "Marshall managed exception ocurred");

            base.Logger.LogCritical(ex, "Marshall managed exception ocurred");
        }
    }
}
