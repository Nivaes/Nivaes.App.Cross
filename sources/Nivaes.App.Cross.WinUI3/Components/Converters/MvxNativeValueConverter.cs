namespace Nivaes.App.Cross.WinUI3
{
    using System.Globalization;
    using Microsoft.Extensions.Logging;
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Data;
    using MvvmCross.Logging;
    using Nivaes.App.Cross;

    public class MvxNativeValueConverter
        : IValueConverter
    {
        private readonly IMvxValueConverter _wrapped;

        protected IMvxValueConverter Wrapped => _wrapped;

        public MvxNativeValueConverter(IMvxValueConverter wrapped)
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
            if (toReturn == MvxBindingConstant.DoNothing)
            {
                MvxLogHost.GetLog<MvxNativeValueConverter>()?.Log(
                    LogLevel.Trace, "DoNothing does not have an equivalent in WinRT - returning UnsetValue instead");
                return DependencyProperty.UnsetValue;
            }

            if (toReturn == MvxBindingConstant.UnsetValue)
            {
                return DependencyProperty.UnsetValue;
            }

            return toReturn;
        }
    }

    public class MvxNativeValueConverter<T>
        : MvxNativeValueConverter
        where T : IMvxValueConverter, new()
    {
        protected new T Wrapped => (T)base.Wrapped;

        public MvxNativeValueConverter()
            : base(new T())
        {
        }
    }
}
