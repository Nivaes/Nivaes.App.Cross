namespace Nivaes.App.Cross.AppKitOS
{
    public interface IMvxTabViewController
    {
        void ShowTabView(NSViewController viewController, string tabTitle);

        bool CloseTabView(ICrossViewModel viewModel);
    }
}
