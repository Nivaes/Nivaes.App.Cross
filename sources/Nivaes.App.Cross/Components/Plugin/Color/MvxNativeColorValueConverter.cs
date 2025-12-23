namespace Nivaes.App.Cross.Color
{
    using System.Globalization;
    using MvvmCross;

    [Preserve(AllMembers = true)]
    public class MvxNativeColorValueConverter : MvxColorValueConverter<System.Drawing.Color>
    {
        protected override System.Drawing.Color Convert(System.Drawing.Color value, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
