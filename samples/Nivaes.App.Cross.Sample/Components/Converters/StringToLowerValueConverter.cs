using System.Globalization;

namespace Nivaes.App.Cross.Sample;

public sealed class StringToLowerValueConverter : CrossValueConverter<string, string>
{
    public StringToLowerValueConverter()
    { }

    protected override string Convert(string value, Type? targetType, object? parameter, CultureInfo? culture)
    {
        return value.ToLower();
    }
}

public sealed class StringToUpperValueConverter : CrossValueConverter<string, string>
{
    public StringToUpperValueConverter() 
    { }

    protected override string Convert(string value, Type? targetType, object? parameter, CultureInfo? culture)
    {
        return value.ToUpper();
    }
}
