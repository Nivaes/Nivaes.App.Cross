namespace MvvmCross.Platforms.Ios.Views
{
    using MvvmCross.Platforms.Ios.Presenters.Attributes;
    using MvvmCross.Presenters.Attributes;
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross;
    using UIKit;

    public interface IMvxSplitViewController
    {
        void ShowMasterView(UIViewController viewController, MvxSplitViewPresentationAttribute attribute);

        void ShowDetailView(UIViewController viewController, MvxSplitViewPresentationAttribute attribute);

        bool CloseChildViewModel(ICrossViewModel viewModel, MvxBasePresentationAttribute attribute);
    }
}
