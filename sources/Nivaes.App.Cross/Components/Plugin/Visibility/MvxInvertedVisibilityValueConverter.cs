namespace MvvmCross.Plugin.Visibility
{
    using System.Globalization;
    using MvvmCross.UI;
    using Nivaes.App.Cross;

    [Preserve(AllMembers = true)]
    public class MvxInvertedVisibilityValueConverter : MvxVisibilityValueConverter
    {
        protected override MvxVisibility Convert(object value, object parameter, CultureInfo culture)
        {
            bool hide = parameter.ConvertToBooleanCore();
            switch (base.Convert(value, parameter, culture))
            {
                case MvxVisibility.Visible when hide:
                    return MvxVisibility.Hidden;
                case MvxVisibility.Visible when !hide:
                    return MvxVisibility.Collapsed;
                default:
                    return MvxVisibility.Visible;
            }
        }
    }
}
