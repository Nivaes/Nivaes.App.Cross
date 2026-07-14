using Nivaes.App.Cross.WinUI;

namespace Nivaes.App.Cross.Sample.WinUI;

[RegionPresentation("NestedFrame")]
public sealed partial class SecondChildView : SecondChildViewPage
{
    public SecondChildView()
    {
        InitializeComponent();
    }
}

public abstract class SecondChildViewPage
    : CrossWindowsPage<SecondChildViewModel>
{
}
