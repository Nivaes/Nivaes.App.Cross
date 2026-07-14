namespace Nivaes.App.Cross.UIKitLib
{
    public interface IMvxSplitViewController
    {
        void ShowMasterView(UIViewController viewController, SplitViewPresentationAttribute attribute);

        void ShowDetailView(UIViewController viewController, SplitViewPresentationAttribute attribute);

        bool CloseChildViewModel(ICrossViewModel viewModel, BasePresentationAttribute attribute);
    }
}
