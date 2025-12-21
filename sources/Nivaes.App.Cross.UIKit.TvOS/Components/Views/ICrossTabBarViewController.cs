namespace Nivaes.App.Cross.UIKit.TvOS
{
    using UIKit;

    public interface ICrossTabBarViewController
    {
        void ShowTabView(UIViewController viewController, CrossTabPresentationAttribute attribute);

        bool ShowChildView(UIViewController viewController);

        bool CloseChildViewModel(ICrossViewModel viewModel);

        bool CloseTabViewModel(ICrossViewModel viewModel);

        bool CanShowChildView();
    }
}
