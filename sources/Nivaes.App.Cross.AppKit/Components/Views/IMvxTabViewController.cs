namespace MvvmCross.Platforms.Mac.Views
{
    using AppKit;
    using Nivaes.App.Cross;

    public interface IMvxTabViewController
    {
        void ShowTabView(NSViewController viewController, string tabTitle);

        bool CloseTabView(ICrossViewModel viewModel);
    }
}
