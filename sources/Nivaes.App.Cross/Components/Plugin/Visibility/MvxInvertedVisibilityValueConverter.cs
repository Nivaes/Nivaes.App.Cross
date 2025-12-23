namespace Nivaes.App.Cross.Visibility
{
    using System.Globalization;
    using MvvmCross;
    using Nivaes.App.Cross;

    [Preserve(AllMembers = true)]
    public class MvxInvertedVisibilityValueConverter : MvxVisibilityValueConverter
    {
        protected override CrossVisibility Convert(object value, object parameter, CultureInfo culture)
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
