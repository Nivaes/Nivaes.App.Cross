using Nivaes.App.Cross.WinUI3;

namespace Nivaes.App.Cross.Sample.WinUI3;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
[MvxPagePresentation]
public sealed partial class SplitRootView 
    : SplitRootViewPage
{
    public SplitRootView()
    {
        this.InitializeComponent();
    }
}

public abstract class SplitRootViewPage
    : CrossWindowsPage<SplitRootViewModel>
{
}