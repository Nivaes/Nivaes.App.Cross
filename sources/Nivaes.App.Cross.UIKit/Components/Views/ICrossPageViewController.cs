namespace Nivaes.App.Cross.UIKit
{
    public interface ICrossPageViewController
    {
        void AddPage(UIViewController viewController, CrossPagePresentationAttribute attribute);

        bool RemovePage(ICrossViewModel viewModel);
    }
}
