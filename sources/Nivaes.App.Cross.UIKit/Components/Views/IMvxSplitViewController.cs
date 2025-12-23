namespace Nivaes.App.Cross.UIKit
{
    public interface IMvxSplitViewController
    {
        void ShowMasterView(UIViewController viewController, MvxSplitViewPresentationAttribute attribute);

        void ShowDetailView(UIViewController viewController, MvxSplitViewPresentationAttribute attribute);

        bool CloseChildViewModel(ICrossViewModel viewModel, CrossBasePresentationAttribute attribute);
    }
}
