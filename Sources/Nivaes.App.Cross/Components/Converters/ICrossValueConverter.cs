using System.Globalization;

namespace Nivaes.App.Cross
{
    public interface ICrossValueConverter
    {
        object? Convert(object? value, Type? targetType, object? parameter, CultureInfo? culture);

        object? ConvertBack(object? value, Type? targetType, object? parameter, CultureInfo? culture);
    }
}