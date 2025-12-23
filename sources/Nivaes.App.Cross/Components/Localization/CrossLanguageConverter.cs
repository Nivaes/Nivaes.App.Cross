namespace Nivaes.App.Cross
{
    using System.Globalization;

    public class CrossLanguageConverter
        : MvxValueConverter
    {
        public override object Convert(object value, Type? targetType, object? parameter, CultureInfo? culture)
        {
            if (value is not ICrossLanguageBinder binder)
                return MvxBindingConstant.UnsetValue;

            if (parameter == null)
                return MvxBindingConstant.UnsetValue;

            var translatedText = binder.GetText(parameter.ToString() ?? string.Empty);
            return translatedText ?? (object)MvxBindingConstant.UnsetValue;
        }
    }
}