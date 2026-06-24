using System.Globalization;
using Microsoft.Extensions.Logging;
using MvvmCross;
using Nivaes.App.Cross;

namespace Nivaes.App.Cross.Visibility
{
    public class CrossInvertedVisibilityValueConverter : CrossVisibilityValueConverter
    {
        public CrossInvertedVisibilityValueConverter(ICrossNativeVisibility nativeVisibility, ILogger<CrossInvertedVisibilityValueConverter> logger)
        : base(nativeVisibility, logger)
        {
        }

        protected override CrossVisibility Convert(object value, object? parameter, CultureInfo? culture)
        {
            bool hide = parameter.ConvertToBooleanCore();
            switch (base.Convert(value, parameter, culture))
            {
                case CrossVisibility.Visible when hide:
                    return CrossVisibility.Hidden;
                case CrossVisibility.Visible when !hide:
                    return CrossVisibility.Collapsed;
                default:
                    return CrossVisibility.Visible;
            }
        }
    }
}
