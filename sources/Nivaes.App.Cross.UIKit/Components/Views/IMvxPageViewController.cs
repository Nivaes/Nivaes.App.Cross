namespace MvvmCross.Platforms.Ios.Views
{
    using MvvmCross.Platforms.Ios.Presenters.Attributes;
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross;
    using UIKit;

    public interface IMvxPageViewController
    {
        void AddPage(UIViewController viewController, MvxPagePresentationAttribute attribute);

        bool RemovePage(ICrossViewModel viewModel);
    }
}
