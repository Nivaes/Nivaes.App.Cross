namespace Nivaes.App.Cross.WinUI
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Reflection;
    using Microsoft.UI.Xaml.Data;

    public class EnumToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
                return null;

            Type type = value.GetType();
            if (!type.IsEnum)
                return value;

            FieldInfo fieldInfo = type.GetField(value.ToString());

            DisplayAttribute displayAttribute = ((DisplayAttribute[])(fieldInfo.GetCustomAttributes(typeof(DisplayAttribute), false))).FirstOrDefault();

            string name = string.Empty;

            if (displayAttribute != null)
            {
                try { name = displayAttribute.GetName(); }
                catch (InvalidOperationException) { }
            }

            if (string.IsNullOrEmpty(name))
                name = value.ToString();

            return name;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return new NotSupportedException();
        }
    }
}
