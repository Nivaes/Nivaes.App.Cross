using System.Globalization;
using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross;

public class CrossRGBIntColorValueConverter : CrossColorValueConverter<int>
{
    public CrossRGBIntColorValueConverter(ICrossNativeColor nativeColor, ILogger<CrossRGBIntColorValueConverter> logger) 
        : base(nativeColor, logger)
    {
    }

    protected override System.Drawing.Color Convert(int value, object? parameter, CultureInfo? culture)
    {
        CrossHexParser.ParseRGBInteger(value, out int red, out int green, out int blue);

        var color = System.Drawing.Color.FromArgb(red, green, blue);

        return color;
    }
}
