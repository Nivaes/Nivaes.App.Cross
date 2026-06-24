using System.Globalization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross;

public abstract class CrossColorValueConverter : CrossValueConverter
{
    private readonly ICrossNativeColor _nativeColor;

    public CrossColorValueConverter(ICrossNativeColor nativeColor, ILogger logger)
        :base(logger)
    {
        _nativeColor = nativeColor;
    }

    protected abstract System.Drawing.Color Convert(object value, object? parameter, CultureInfo? culture);

    public sealed override object Convert(object value, Type? targetType, object? parameter,
        CultureInfo? culture)
    {
        return _nativeColor.ToNative(Convert(value, parameter, culture)) ?? CrossBindingConstant.UnsetValue;
    }
}

public abstract class CrossColorValueConverter<T> : CrossColorValueConverter
{
    public CrossColorValueConverter(ICrossNativeColor nativeColor, ILogger logger)
        :base(nativeColor, logger)
    { }

    protected sealed override System.Drawing.Color Convert(object value, object? parameter, CultureInfo? culture)
    {
        if (value is T t)
            return Convert(t, parameter, culture);

        return default;
    }

    protected abstract System.Drawing.Color Convert(T value, object? parameter, CultureInfo? culture);
}