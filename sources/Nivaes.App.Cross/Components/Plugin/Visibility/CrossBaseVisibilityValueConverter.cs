using System.Globalization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nivaes.App.Cross;
using Nivaes.IoC;

namespace Nivaes.App.Cross.Visibility;

public abstract class CrossBaseVisibilityValueConverter<T>
    : CrossBaseVisibilityValueConverter
{
    public CrossBaseVisibilityValueConverter(ICrossNativeVisibility nativeVisibility, ILogger logger)
        : base(nativeVisibility, logger)
    {
    }

    protected sealed override CrossVisibility Convert(object value, object? parameter, CultureInfo? culture)
    {
        return Convert((T)value, parameter, culture);
    }

    protected abstract CrossVisibility Convert(T value, object? parameter, CultureInfo? culture);
}

public abstract class CrossBaseVisibilityValueConverter
    : CrossValueConverter
{
    private ICrossNativeVisibility _nativeVisibility;

    public CrossBaseVisibilityValueConverter(ICrossNativeVisibility nativeVisibility, ILogger logger) 
        : base(logger)
    {
        _nativeVisibility = nativeVisibility;
    }

    protected abstract CrossVisibility Convert(object value, object? parameter, CultureInfo? culture);

    public sealed override object Convert(object value, Type? targetType, object? parameter, CultureInfo? culture)
    {
        var mvx = Convert(value, parameter, culture);
        return _nativeVisibility.ToNative(mvx);
    }

    public sealed override object ConvertBack(object value, Type? targetType, object? parameter, CultureInfo? culture)
    {
        return base.ConvertBack(value, targetType, parameter, culture);
    }
}
