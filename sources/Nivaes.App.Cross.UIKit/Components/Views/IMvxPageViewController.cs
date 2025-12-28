namespace Nivaes.App.Cross.UIKitOS
{
    public interface IMvxPageViewController
    {
        void AddPage(UIViewController viewController, MvxPagePresentationAttribute attribute);

        bool RemovePage(ICrossViewModel viewModel);
    }
}
