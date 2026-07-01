using System.Globalization;
using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross
{
    public class DebuggerLaunchConverter
        : CrossValueConverter
    {
        public DebuggerLaunchConverter(ILogger<DebuggerLaunchConverter> logger)
           : base(logger)
        { }

        public override object Convert(object value, Type? targetType, object? parameter, CultureInfo? culture)
        {
            System.Diagnostics.Debugger.Launch();

            return value;
        }

        public override object ConvertBack(object value, Type? targetType, object? parameter, CultureInfo? culture)
        {
            System.Diagnostics.Debugger.Launch();

            return value;
        }
    }
}
