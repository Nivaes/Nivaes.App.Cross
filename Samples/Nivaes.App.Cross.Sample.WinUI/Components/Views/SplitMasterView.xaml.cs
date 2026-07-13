using Nivaes.App.Cross.WinUI;
using Nivaes.App.Cross.WinUI.Components.Presenters.Attributes;

namespace Nivaes.App.Cross.Sample.WinUI;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
[MvxSplitViewPresentation(Position = SplitPanePosition.Pane)]
public sealed partial class SplitMasterView
    : SplitMasterViewPage
{
    public SplitMasterView()
    {
        this.InitializeComponent();
    }
}

public abstract class SplitMasterViewPage
    : CrossWindowsPage<SplitMasterViewModel>
{
}