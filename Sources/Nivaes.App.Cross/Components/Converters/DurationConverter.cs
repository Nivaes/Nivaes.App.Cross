using System.Globalization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nivaes.App.Cross.Observability;

namespace Nivaes.App.Cross
{
    public class DurationConverter
        : CrossValueConverter<int, string>
    {
        public DurationConverter()
            : this(CrossLoggerHost.GetLogger<DurationConverter>())
        { }

        [ActivatorUtilitiesConstructor]
        public DurationConverter(ILogger<DurationConverter> logger)
           : base(logger)
        { }

        protected override string? Convert(int value, Type? targetType, object? parameter, CultureInfo? culture)
        {
            return string.Format(CultureInfo.CurrentCulture, "{0:00} min", value);
        }
    }
}
