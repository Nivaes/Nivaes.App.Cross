namespace Nivaes.App.Cross.WinUI
{
    using System;
    using Microsoft.UI.Xaml.Data;

    public class DebugValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return "DebugValueConverter:  " + value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value;
        }
    }
}
