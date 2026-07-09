namespace Nivaes.App.Cross.Visibility;

using System.Globalization;
using Microsoft.Extensions.Logging;

[CrossValueConverter(Name = "Visibility")]
public class CrossVisibilityValueConverter
    : CrossBaseVisibilityValueConverter
{
    public CrossVisibilityValueConverter(ICrossNativeVisibility nativeVisibility, ILogger<CrossVisibilityValueConverter> logger)
        : base(nativeVisibility, logger)
    {
    }

    protected override CrossVisibility Convert(object value, object? parameter, CultureInfo? culture)
    {
        bool visible = value.ConvertToBooleanCore();
        bool hide = parameter.ConvertToBooleanCore();

        if (!visible)
        {
            return hide ? CrossVisibility.Hidden : CrossVisibility.Collapsed;
        }

        return CrossVisibility.Visible;
    }
}
