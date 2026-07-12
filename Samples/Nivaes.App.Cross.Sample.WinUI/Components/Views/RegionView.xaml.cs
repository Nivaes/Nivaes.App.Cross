using Nivaes.App.Cross.WinUI;

namespace Nivaes.App.Cross.Sample.WinUI;

[MvxRegionPresentation("PopupLocation")]
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class RegionView : RegionViewPage
{
    public RegionView()
    {
        this.InitializeComponent();
    }
}

public abstract class RegionViewPage
    : CrossWindowsPage<RegionViewModel>;
