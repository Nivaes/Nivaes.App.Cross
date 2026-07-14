namespace Nivaes.App.Cross.UIKitLib
{
    public interface IMvxPageViewController
    {
        void AddPage(UIViewController viewController, PagePresentationAttribute attribute);

        bool RemovePage(ICrossViewModel viewModel);
    }
}
