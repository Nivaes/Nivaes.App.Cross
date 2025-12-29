using Nivaes.App.Cross.WinUI3;

namespace Nivaes.App.Cross.Sample.WinUI3;

[MvxViewFor(typeof(RegionViewModel))]
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
    : MvxWindowsPage<RegionViewModel>;
