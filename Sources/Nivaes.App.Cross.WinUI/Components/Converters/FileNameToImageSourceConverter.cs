namespace Nivaes.App.Cross.WinUI
{
    using System;
    using Microsoft.UI.Xaml.Data;
    using Microsoft.UI.Xaml.Media.Imaging;

    public class FileNameToImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is string str && !string.IsNullOrEmpty(str))
            {
                return new BitmapImage(new Uri(str, UriKind.Absolute));
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotSupportedException();
        }
    }
}
