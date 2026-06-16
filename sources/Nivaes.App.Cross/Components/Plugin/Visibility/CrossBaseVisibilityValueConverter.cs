using System.Globalization;
using Microsoft.Extensions.DependencyInjection;
using Nivaes.App.Cross;
using Nivaes.IoC;

namespace Nivaes.App.Cross.Visibility;

public abstract class CrossBaseVisibilityValueConverter<T>
    : MvxBaseVisibilityValueConverter
{
    protected sealed override CrossVisibility Convert(object value, object parameter, CultureInfo culture)
    {
        return Convert((T)value, parameter, culture);
    }

    protected abstract CrossVisibility Convert(T value, object parameter, CultureInfo culture);
}

public abstract class MvxBaseVisibilityValueConverter
    : CroosValueConverter
{
    private ICrossNativeVisibility _nativeVisibility;

    private ICrossNativeVisibility NativeVisibility => _nativeVisibility ??= IPlatformApplication.Current!.Services.GetRequiredService<ICrossNativeVisibility>();

    protected abstract CrossVisibility Convert(object value, object parameter, CultureInfo culture);

    public sealed override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var mvx = Convert(value, parameter, culture);
        return NativeVisibility.ToNative(mvx);
    }

    public sealed override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return base.ConvertBack(value, targetType, parameter, culture);
    }
}
