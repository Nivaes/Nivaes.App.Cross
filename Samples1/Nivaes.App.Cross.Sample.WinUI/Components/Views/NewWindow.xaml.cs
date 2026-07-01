using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Nivaes.App.Cross.WinUI;

namespace Nivaes.App.Cross.Sample.WinUI;

[MvxViewFor(typeof(NewWindowViewModel))]
[MvxNewWindowPresentation]
public sealed partial class NewWindow
    : NewWindowPage, IMvxNeedWindow
{
    public NewWindow()
    {
        this.InitializeComponent();
        this.PopupLocation.Navigate(typeof(BlankPage));
    }

    public void SetWindow(Window window, AppWindow appWindow)
    {
        this.AppWindow = appWindow;
        AppWindowUtils.SetTitleBar(appWindow, "Hello new window");
    }

    public AppWindow? AppWindow { get; set; }

    public bool CanClose()
    {
        return true;
    }
}


public abstract class NewWindowPage
    : CrossWindowsPage<NewWindowViewModel>
{
}
