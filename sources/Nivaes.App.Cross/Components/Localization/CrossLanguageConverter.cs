namespace Nivaes.App.Cross
{
    using System;
    using System.Globalization;

    [Obsolete]
    public class CrossLanguageConverter
        : CrossValueConverter
    {
        public override object Convert(object value, Type? targetType, object? parameter, CultureInfo? culture)
        {
            if (value is not ICrossLanguageBinder binder)
                return CrossBindingConstant.UnsetValue;

            if (parameter == null)
                return CrossBindingConstant.UnsetValue;

            var translatedText = binder.GetText(parameter.ToString() ?? string.Empty);
            return translatedText ?? (object)CrossBindingConstant.UnsetValue;
        }
    }
}