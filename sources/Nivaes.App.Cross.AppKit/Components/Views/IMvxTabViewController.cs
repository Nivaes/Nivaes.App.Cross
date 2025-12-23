namespace Nivaes.App.Cross.AppKit
{
    public interface IMvxTabViewController
    {
        void ShowTabView(NSViewController viewController, string tabTitle);

        bool CloseTabView(ICrossViewModel viewModel);
    }
}
