using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross;

[CrossValueConverter(Name = "RGBA")]
public class CrossRGBAValueConverter
    : CrossRGBValueConverter
{
    public CrossRGBAValueConverter(ICrossNativeColor nativeColor, ILogger<CrossRGBAValueConverter> logger)
        : base(nativeColor, logger)
    { }
}
