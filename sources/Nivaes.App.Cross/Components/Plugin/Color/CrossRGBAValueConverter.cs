using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross;

public class CrossRGBAValueConverter 
    : CrossRGBValueConverter
{
    public CrossRGBAValueConverter(ICrossNativeColor nativeColor, ILogger<CrossRGBAValueConverter> logger) 
        : base(nativeColor, logger) 
    { }
}
