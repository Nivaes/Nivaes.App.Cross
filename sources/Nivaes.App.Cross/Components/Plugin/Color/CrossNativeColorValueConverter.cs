using System.Globalization;
using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross;

public class CrossNativeColorValueConverter : CrossColorValueConverter<System.Drawing.Color>
{
    public CrossNativeColorValueConverter(ICrossNativeColor nativeColor, ILogger<CrossNativeColorValueConverter> logger)
        :base(nativeColor, logger)
    {
    }

    protected override System.Drawing.Color Convert(System.Drawing.Color value, object? parameter, CultureInfo? culture)
    {
        return value;
    }
}
