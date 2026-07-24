namespace Nivaes.App.Cross.WinUI
{
    using System;
    using Microsoft.UI.Xaml.Data;

    public class InvertBooleanValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is bool)
                return !(bool)value;
            else
                return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is bool)
                return !(bool)value;
            else
                return false;
        }
    }
}
