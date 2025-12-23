namespace Nivaes.App.Cross.Color
{
    using System.Globalization;
    using MvvmCross;

    public abstract class MvxColorValueConverter : MvxValueConverter
    {
        private readonly Lazy<ICrossNativeColor?> _nativeColor = new(() => Mvx.IoCProvider?.Resolve<ICrossNativeColor>());

        protected abstract System.Drawing.Color Convert(object value, object? parameter, CultureInfo? culture);

        public sealed override object Convert(object value, Type? targetType, object? parameter,
            CultureInfo? culture)
        {
            return _nativeColor.Value?.ToNative(Convert(value, parameter, culture)) ?? MvxBindingConstant.UnsetValue;
        }
    }

    public abstract class MvxColorValueConverter<T> : MvxColorValueConverter
    {
        protected sealed override System.Drawing.Color Convert(object value, object? parameter, CultureInfo? culture)
        {
            if (value is T t)
                return Convert(t, parameter, culture);

            return default;
        }

        protected abstract System.Drawing.Color Convert(T value, object? parameter, CultureInfo? culture);
    }
}