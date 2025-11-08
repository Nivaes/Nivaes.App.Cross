namespace Nivaes.App.Cross.UIKit
{
    public interface ICrossSplitViewController
    {
        void ShowMasterView(UIViewController viewController, CrossSplitViewPresentationAttribute attribute);

        void ShowDetailView(UIViewController viewController, CrossSplitViewPresentationAttribute attribute);

        bool CloseChildViewModel(ICrossViewModel viewModel, CrossBasePresentationAttribute attribute);
    }
}
