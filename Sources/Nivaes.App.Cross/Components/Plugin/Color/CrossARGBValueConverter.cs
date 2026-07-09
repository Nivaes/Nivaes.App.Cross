using System.Globalization;
using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross;

[CrossValueConverter(Name = "ARGB")]
public class CrossARGBValueConverter : CrossColorValueConverter<string>
{
    public CrossARGBValueConverter(ICrossNativeColor nativeColor, ILogger<CrossARGBValueConverter> logger)
        : base(nativeColor, logger)
    { }

    protected override System.Drawing.Color Convert(string value, object? parameter, CultureInfo? culture)
        => CrossHexParser.ColorFromHexString(value, assumeArgb: true);
}
