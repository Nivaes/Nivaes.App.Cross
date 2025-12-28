using System.Globalization;

namespace Nivaes.App.Cross;

[Preserve(AllMembers = true)]
public class CrossNativeColorValueConverter : MvxColorValueConverter<System.Drawing.Color>
{
    protected override System.Drawing.Color Convert(System.Drawing.Color value, object parameter, CultureInfo culture)
    {
        return value;
    }
}
