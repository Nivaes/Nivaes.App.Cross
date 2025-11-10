namespace Nivaes.App.Cross.Sample.WinUI3
{
    using System;
    using Microsoft.UI.Xaml.Data;

    public class RootViewValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return "RootViewValueConverter:  " + value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value;
        }
    }
}
