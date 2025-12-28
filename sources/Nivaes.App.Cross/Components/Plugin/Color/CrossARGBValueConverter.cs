using System.Globalization;

namespace Nivaes.App.Cross;

[Preserve(AllMembers = true)]
public class CrossARGBValueConverter : MvxColorValueConverter<string>
{
    protected override System.Drawing.Color Convert(string value, object parameter, CultureInfo culture)
        => CrossHexParser.ColorFromHexString(value, assumeArgb: true);
}
