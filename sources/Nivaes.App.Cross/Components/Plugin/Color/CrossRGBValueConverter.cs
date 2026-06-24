using System.Globalization;
using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross;

public class CrossRGBValueConverter : CrossColorValueConverter<string>
{
    public CrossRGBValueConverter(ICrossNativeColor nativeColor, ILogger<CrossRGBValueConverter> logger) 
        : base(nativeColor, logger) 
    { }

    protected override System.Drawing.Color Convert(string value, object? parameter, CultureInfo? culture)
        => CrossHexParser.ColorFromHexString(value);
}
