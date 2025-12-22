namespace MvvmCross.Plugin.Visibility
{
    using System.Globalization;
    using Nivaes.App.Cross;

    [Preserve(AllMembers = true)]
    public class MvxVisibilityValueConverter : MvxBaseVisibilityValueConverter
    {
        protected override CrossVisibility Convert(object value, object parameter, CultureInfo culture)
        {
            bool visible = value.ConvertToBooleanCore();
            bool hide = parameter.ConvertToBooleanCore();

            if (!visible)
            {
                return hide ? CrossVisibility.Hidden : CrossVisibility.Collapsed;
            }

            return CrossVisibility.Visible;
        }
    }
}
