using Nivaes.App.Cross.WinUI3;

namespace Nivaes.App.Cross.Sample.WinUI3;

[MvxRegionPresentation("NestedFrame")]
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
