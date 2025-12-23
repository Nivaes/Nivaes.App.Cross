namespace Nivaes.App.Cross.UIKit
{
    public interface IMvxPageViewController
    {
        void AddPage(UIViewController viewController, MvxPagePresentationAttribute attribute);

        bool RemovePage(ICrossViewModel viewModel);
    }
}
