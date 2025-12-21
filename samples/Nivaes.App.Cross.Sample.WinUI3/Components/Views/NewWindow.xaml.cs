namespace Playground.WinUi3.Views
{
    using Microsoft.UI.Windowing;
    using Microsoft.UI.Xaml;
    using MvvmCross.Platforms.WinUi.Presenters.Attributes;
    using MvvmCross.Platforms.WinUi.Presenters.Models;
    using MvvmCross.Platforms.WinUi.Presenters.Utils;
    using MvvmCross.Platforms.WinUi.Views;
    using Nivaes.App.Cross;
    using Playground.Core.ViewModels;

    [MvxViewFor(typeof(NewWindowViewModel))]
    [MvxNewWindowPresentation]
    public sealed partial class NewWindow : NewWindowPage, IMvxNeedWindow
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

        public AppWindow AppWindow { get; set; }

        public bool CanClose()
        {
            return true;
        }
    }


    public abstract class NewWindowPage : MvxWindowsPage<NewWindowViewModel>
    {
    }
}
