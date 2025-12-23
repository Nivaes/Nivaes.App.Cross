namespace Nivaes.App.Cross
{
    using System.Globalization;
    using System.Windows.Input;

    public class CrossCommandParameterValueConverter
        : MvxValueConverter<ICommand, ICommand>
    {
        protected override ICommand Convert(ICommand value, Type? targetType, object? parameter,
                                            CultureInfo? culture)
        {
            return new CrossWrappingCommand(value, parameter);
        }
    }
}
