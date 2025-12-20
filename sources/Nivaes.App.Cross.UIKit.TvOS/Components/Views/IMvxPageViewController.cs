namespace MvvmCross.Platforms.Tvos.Views
{
    using MvvmCross.Platforms.Tvos.Presenters.Attributes;
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross;
    using UIKit;

    public interface IMvxPageViewController
    {
        void AddPage(UIViewController viewController, MvxPagePresentationAttribute attribute);

        bool RemovePage(ICrossViewModel viewModel);
    }
}
