using Nivaes.App.Cross.WinUI;
using Nivaes.App.Cross.WinUI.Components.Presenters.Attributes;

namespace Nivaes.App.Cross.Sample.WinUI;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
[MvxSplitViewPresentation(Position = SplitPanePosition.Content)]
public sealed partial class SplitDetailView : SplitDetailViewPage
{
    public SplitDetailView()
    {
        this.InitializeComponent();
    }
}

public abstract class SplitDetailViewPage : CrossWindowsPage<SplitDetailViewModel>
{
}
