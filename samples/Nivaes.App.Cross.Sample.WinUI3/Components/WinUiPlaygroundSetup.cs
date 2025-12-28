using Microsoft.Extensions.Logging;
using Nivaes.App.Cross.WinUI3;
using Serilog;
using Serilog.Extensions.Logging;

namespace Nivaes.App.Cross.Sample.WinUI3;

public class WinUiPlaygroundSetup : MvxWindowsSetup<Nivaes.App.Cross.Sample.App>
{
    protected override ILoggerProvider CreateLogProvider()
    {
        return new SerilogLoggerProvider();
    }

    protected override ILoggerFactory CreateLogFactory()
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .CreateLogger();

        return new SerilogLoggerFactory();
    }
}
