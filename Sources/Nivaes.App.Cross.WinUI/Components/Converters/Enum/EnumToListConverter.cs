namespace Nivaes.App.Cross.WinUI
{
    using System;
    using Microsoft.UI.Xaml.Data;

    public class EnumToListConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
                return null;

            Type type = value.GetType();

            var enumValues = Enum.GetValues(type);

            return enumValues;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return new NotSupportedException();
        }
    }
}
