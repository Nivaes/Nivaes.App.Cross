namespace Nivaes.App.Cross.UIKitOS
{
    public interface IMvxTabBarViewController
    {
        void ShowTabView(UIViewController viewController, MvxTabPresentationAttribute attribute);

        bool ShowChildView(UIViewController viewController);

        bool CloseChildViewModel(ICrossViewModel viewModel);

        bool CloseTabViewModel(ICrossViewModel viewModel);

        bool CanShowChildView();
    }
}
