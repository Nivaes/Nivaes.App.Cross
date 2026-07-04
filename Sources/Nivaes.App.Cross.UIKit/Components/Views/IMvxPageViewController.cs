namespace Nivaes.App.Cross.UIKitLib
{
    public interface IMvxPageViewController
    {
        void AddPage(UIViewController viewController, MvxPagePresentationAttribute attribute);

        bool RemovePage(ICrossViewModel viewModel);
    }
}
