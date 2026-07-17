namespace Nivaes.App.Cross.UIKitLib
{
    public interface ITabBarViewController
    {
        void ShowTabView(UIViewController viewController, TabPresentationAttribute attribute);

        bool ShowChildView(UIViewController viewController);

        bool CloseChildViewModel(ICrossViewModel viewModel);

        bool CloseTabViewModel(ICrossViewModel viewModel);

        bool CanShowChildView();
    }
}
