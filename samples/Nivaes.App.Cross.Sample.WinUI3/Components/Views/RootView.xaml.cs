using Nivaes.App.Cross;
using Playground.Core.ViewModels;
using Nivaes.App.Cross.WinUI3;

namespace Nivaes.App.Cross.Sample.WinUI3;

[MvxViewFor(typeof(RootViewModel))]
[MvxPagePresentation]
public sealed partial class RootView : RootViewPage
{
    public RootView()
    {
        this.InitializeComponent();
        this.PopupLocation.Navigate(typeof(BlankPage));
    }
}

public abstract class RootViewPage : CrossWindowsPage<RootViewModel>
{
}
