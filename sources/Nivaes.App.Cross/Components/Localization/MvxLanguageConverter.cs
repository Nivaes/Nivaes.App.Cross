namespace Nivaes.App.Cross
{
    using System.Globalization;

    public class MvxLanguageConverter
        : MvxValueConverter
    {
        public override object Convert(object value, Type? targetType, object? parameter, CultureInfo? culture)
        {
            if (value is not IMvxLanguageBinder binder)
                return MvxBindingConstant.UnsetValue;

            if (parameter == null)
                return MvxBindingConstant.UnsetValue;

            var translatedText = binder.GetText(parameter.ToString() ?? string.Empty);
            return translatedText ?? (object)MvxBindingConstant.UnsetValue;
        }
    }
}