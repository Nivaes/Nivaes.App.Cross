namespace MvvmCross.Plugin.Visibility
{
    using System.Globalization;
    using MvvmCross.UI;
    using Nivaes.App.Cross;

    [Preserve(AllMembers = true)]
    public class MvxVisibilityValueConverter : MvxBaseVisibilityValueConverter
    {
        protected override MvxVisibility Convert(object value, object parameter, CultureInfo culture)
        {
            bool visible = value.ConvertToBooleanCore();
            bool hide = parameter.ConvertToBooleanCore();

            if (!visible)
            {
                return hide ? MvxVisibility.Hidden : MvxVisibility.Collapsed;
            }

            return MvxVisibility.Visible;
        }
    }
}
