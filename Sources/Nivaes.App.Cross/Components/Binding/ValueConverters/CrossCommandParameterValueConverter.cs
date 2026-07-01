using System.Globalization;
using System.Windows.Input;
using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross
{
    public class CrossCommandParameterValueConverter
        : CrossValueConverter<ICommand, ICommand>
    {
        public CrossCommandParameterValueConverter(ILogger<CrossCommandParameterValueConverter> logger)
            : base(logger)
        { }

        protected override ICommand Convert(ICommand value, Type? targetType, object? parameter,
                                            CultureInfo? culture)
        {
            return new CrossWrappingCommand(value, parameter);
        }
    }
}
