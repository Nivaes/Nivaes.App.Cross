namespace MvvmCross.Platforms.Tvos.Views
{
    using MvvmCross.Platforms.Tvos.Presenters.Attributes;
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross;
    using UIKit;

    public interface IMvxTabBarViewController
    {
        void ShowTabView(UIViewController viewController, MvxTabPresentationAttribute attribute);

        bool ShowChildView(UIViewController viewController);

        bool CloseChildViewModel(ICrossViewModel viewModel);

        bool CloseTabViewModel(ICrossViewModel viewModel);

        bool CanShowChildView();
    }
}
