using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross
{
    public class DisplayDescriptionConverter
        : CrossValueConverter
    {
        public DisplayDescriptionConverter(ILogger<DisplayDescriptionConverter> logger)
            : base(logger)
        { }

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            if (value.GetType()?
                        .GetMember(value.ToString())?
                        .First()?
                        .GetCustomAttributes(typeof(DisplayAttribute), false)?
                        .Single() is DisplayAttribute displayAttribute)
                return displayAttribute.GetDescription();
            else
                return value.ToString();
        }
    }
}
