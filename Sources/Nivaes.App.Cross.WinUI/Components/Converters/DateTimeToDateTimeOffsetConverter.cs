using Microsoft.UI.Xaml.Data;

namespace Nivaes.App.Cross.WinUI
{
    public class DateTimeToDateTimeOffsetConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is DateTimeOffset date)
            {
                return date.DateTime;
            }
            return default(DateTime);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is DateTime date)
            {
                return new DateTimeOffset(date);
            }
            return default(DateTimeOffset);
        }
    }
}
