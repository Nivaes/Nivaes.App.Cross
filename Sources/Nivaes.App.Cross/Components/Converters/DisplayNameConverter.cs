using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Microsoft.Extensions.Logging;
using Nivaes.App.Cross;

namespace Nivaes.App
{
    public class DisplayNameConverter
        : CrossValueConverter
    {
        public DisplayNameConverter(ILogger<DisplayNameConverter> logger)
            : base(logger)
        { }

        public override object Convert(object value, Type? targetType, object? parameter, CultureInfo? culture)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            if (value.GetType()?
                        .GetMember(value.ToString()!)?
                        .First()?
                        .GetCustomAttributes(typeof(DisplayAttribute), false)?
                        .Single() is DisplayAttribute displayAttribute)
                return displayAttribute.GetName()!;
            else
                return value.ToString()!;
        }
    }
}
