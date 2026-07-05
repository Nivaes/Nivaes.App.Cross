using System.Globalization;
using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross
{
    public class DebugConverter
        : CrossValueConverter
    {
        public DebugConverter(ILogger<DebugConverter> logger)
            : base(logger)
        { }

        public override object Convert(object value, Type? targetType, object? parameter, CultureInfo? culture)
        {

            //if (System.Diagnostics.Debugger.IsAttached)
            System.Diagnostics.Debugger.Break();

            return value;
        }

        public override object ConvertBack(object value, Type? targetType, object? parameter, CultureInfo? culture)
        {
            //if (System.Diagnostics.Debugger.IsAttached)
            System.Diagnostics.Debugger.Break();

            return value;
        }
    }
}
