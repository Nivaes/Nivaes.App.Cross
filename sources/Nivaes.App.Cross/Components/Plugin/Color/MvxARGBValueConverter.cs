namespace Nivaes.App.Cross.Color
{
    using System.Globalization;
    using MvvmCross;

    [Preserve(AllMembers = true)]
    public class MvxARGBValueConverter : MvxColorValueConverter<string>
    {
        protected override System.Drawing.Color Convert(string value, object parameter, CultureInfo culture)
            => MvxHexParser.ColorFromHexString(value, assumeArgb: true);
    }
}
