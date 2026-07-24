namespace Nivaes.App.Cross.WinUI
{
    using System;
    using Microsoft.UI.Xaml.Data;

    public class DateTimeOffsetToDateTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is DateTime date)
            {
                return new DateTimeOffset(date);
            }
            return default(DateTimeOffset);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is DateTimeOffset date)
            {
                return date.DateTime;
            }
            return default(DateTime);
        }
    }
}
