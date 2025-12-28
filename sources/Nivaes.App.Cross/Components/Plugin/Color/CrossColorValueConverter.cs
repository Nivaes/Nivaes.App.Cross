using System.Globalization;
using Nivaes.IoC;

namespace Nivaes.App.Cross;
public abstract class CrossColorValueConverter : CroosValueConverter
{
    private readonly Lazy<ICrossNativeColor?> _nativeColor = new(() => Mvx.IoCProvider?.Resolve<ICrossNativeColor>());

    protected abstract System.Drawing.Color Convert(object value, object? parameter, CultureInfo? culture);

    public sealed override object Convert(object value, Type? targetType, object? parameter,
        CultureInfo? culture)
    {
        return _nativeColor.Value?.ToNative(Convert(value, parameter, culture)) ?? CrossBindingConstant.UnsetValue;
    }
}

public abstract class MvxColorValueConverter<T> : CrossColorValueConverter
{
    protected sealed override System.Drawing.Color Convert(object value, object? parameter, CultureInfo? culture)
    {
        if (value is T t)
            return Convert(t, parameter, culture);

        return default;
    }

    protected abstract System.Drawing.Color Convert(T value, object? parameter, CultureInfo? culture);
}