namespace Nivaes.App.Cross.AppKitLib
{
    public interface IMvxTabViewController
    {
        void ShowTabView(NSViewController viewController, string tabTitle);

        bool CloseTabView(ICrossViewModel viewModel);
    }
}
