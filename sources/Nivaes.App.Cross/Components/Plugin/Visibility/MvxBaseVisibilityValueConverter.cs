namespace MvvmCross.Plugin.Visibility
{
    using System.Globalization;
    using MvvmCross.Converters;
    using Nivaes.App.Cross;

    public abstract class MvxBaseVisibilityValueConverter<T>
        : MvxBaseVisibilityValueConverter
    {
        protected sealed override CrossVisibility Convert(object value, object parameter, CultureInfo culture)
        {
            return Convert((T)value, parameter, culture);
        }

        protected abstract CrossVisibility Convert(T value, object parameter, CultureInfo culture);
    }

    public abstract class MvxBaseVisibilityValueConverter
        : MvxValueConverter
    {
        private ICrossNativeVisibility _nativeVisibility;

        private ICrossNativeVisibility NativeVisibility => _nativeVisibility ??= Mvx.IoCProvider.Resolve<ICrossNativeVisibility>();

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
}
