using System.Globalization;
using Microsoft.Extensions.Logging;
using Nivaes.App.Cross;

namespace Nivaes.App
{
    public class GeoDistanceConverter
        : CrossValueConverter<string>
    {
        public GeoDistanceConverter(ILogger<GeoDistanceConverter> logger)
            : base(logger)
        { }


        protected override object? Convert(string geographicalLocalization, Type? targetType, object? parameter, CultureInfo? culture)
        {
            var latitud1 = 40.05568762;
            var longitud1 = -3.08131512;

            var latitud2 = 41.39596444;
            var longitud2 = 2.15669818;

            var distance = GpsHelper.GeoDistance(latitud1, longitud1, latitud2, longitud2);

            return string.Format(culture, "{0:0.0}km", distance);
        }
    }
}
