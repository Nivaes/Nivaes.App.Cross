using System.Globalization;

namespace Nivaes.App.Cross;

[Preserve(AllMembers = true)]
public class CrossRGBIntColorValueConverter : MvxColorValueConverter<int>
{
    protected override System.Drawing.Color Convert(int value, object parameter, CultureInfo culture)
    {
        CrossHexParser.ParseRGBInteger(value, out int red, out int green, out int blue);

        var color = System.Drawing.Color.FromArgb(red, green, blue);

        return color;
    }
}
