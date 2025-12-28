namespace Nivaes.App.Cross.UIKitOS
{
    public interface IMvxSplitViewController
    {
        void ShowMasterView(UIViewController viewController, MvxSplitViewPresentationAttribute attribute);

        void ShowDetailView(UIViewController viewController, MvxSplitViewPresentationAttribute attribute);

        bool CloseChildViewModel(ICrossViewModel viewModel, CrossBasePresentationAttribute attribute);
    }
}
