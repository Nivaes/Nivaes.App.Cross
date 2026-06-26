using System.Globalization;
using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross.Sample;

public sealed class StringToLowerValueConverter : CrossValueConverter<string, string>
{
    public StringToLowerValueConverter(ILogger<StringToLowerValueConverter> logger)
        : base(logger)
    { }

    protected override string Convert(string value, Type? targetType, object? parameter, CultureInfo? culture)
    {
        return value.ToLower();
    }
}

public sealed class StringToUpperValueConverter : CrossValueConverter<string, string>
{
    public StringToUpperValueConverter(ILogger<StringToUpperValueConverter> logger)
        : base(logger)
    { }

    protected override string Convert(string value, Type? targetType, object? parameter, CultureInfo? culture)
    {
        return value.ToUpper();
    }
}
