namespace Nivaes.App.Cross.WinUI3
{
    using System.Globalization;
    using Microsoft.Extensions.Logging;
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Data;

    public class CrossNativeValueConverter
        : IValueConverter
    {
        private readonly ICrossValueConverter _wrapped;

        protected ICrossValueConverter Wrapped => _wrapped;

        public CrossNativeValueConverter(ICrossValueConverter wrapped)
        {
            _wrapped = wrapped;
        }

        public virtual object Convert(object value, Type targetType, object parameter, string language)
        {
            // note - Language ignored here!
            var toReturn = _wrapped.Convert(value, targetType, parameter, CultureInfo.CurrentUICulture);
            return MapIfSpecialValue(toReturn);
        }

        public virtual object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            // note - Language ignored here!
            var toReturn = _wrapped.ConvertBack(value, targetType, parameter, CultureInfo.CurrentUICulture);
            return MapIfSpecialValue(toReturn);
        }

        private static object MapIfSpecialValue(object toReturn)
        {
            if (toReturn == CrossBindingConstant.DoNothing)
            {
                CrossLogHost.GetLog<CrossNativeValueConverter>()?.Log(
                    LogLevel.Trace, "DoNothing does not have an equivalent in WinRT - returning UnsetValue instead");
                return DependencyProperty.UnsetValue;
            }

            if (toReturn == CrossBindingConstant.UnsetValue)
            {
                return DependencyProperty.UnsetValue;
            }

            return toReturn;
        }
    }

    public class CrossNativeValueConverter<T>
        : CrossNativeValueConverter
        where T : ICrossValueConverter, new()
    {
        protected new T Wrapped => (T)base.Wrapped;

        public CrossNativeValueConverter()
            : base(new T())
        {
        }
    }
}
