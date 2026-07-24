using System.Globalization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nivaes.App.Cross.Observability;

namespace Nivaes.App.Cross
{
    public class PriceConverter
        : CrossValueConverter<decimal, string>
    {
        public PriceConverter()
            : this(CrossLoggerHost.GetLogger<PriceConverter>())
        { }

        [ActivatorUtilitiesConstructor]
        public PriceConverter(ILogger<PriceConverter> logger)
            : base(logger)
        { }

        protected override string? Convert(decimal value, Type? targetType, object? parameter, CultureInfo? culture)
        {
            return string.Format(culture, "{0:C}", value);
        }
    }
}
