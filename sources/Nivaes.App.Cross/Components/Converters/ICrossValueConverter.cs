namespace Nivaes.App.Cross
{
    using System;
    using System.Globalization;

    [Obsolete("Quitar IoC de Cross")]
    public interface ICrossValueConverter
    {
        object Convert(
            object value,
            Type? targetType,
            object? parameter,
            CultureInfo? culture);

        object ConvertBack(
            object value,
            Type? targetType,
            object? parameter,
            CultureInfo? culture);
    }
}