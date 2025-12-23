namespace Nivaes.App.Cross
{
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;

    public interface IMvxValueConverter
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