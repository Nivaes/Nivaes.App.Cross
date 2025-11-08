namespace Nivaes.App.Cross.WinUI3
{
    using System;
    using Microsoft.UI.Xaml.Data;

    public class CrossDebugValueConverter : IValueConverter
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
