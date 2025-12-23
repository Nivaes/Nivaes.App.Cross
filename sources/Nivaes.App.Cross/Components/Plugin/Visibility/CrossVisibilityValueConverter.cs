namespace Nivaes.App.Cross.Visibility
{
    using System.Globalization;
    using MvvmCross;

    [Preserve(AllMembers = true)]
    public class CrossVisibilityValueConverter 
        : MvxBaseVisibilityValueConverter
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
