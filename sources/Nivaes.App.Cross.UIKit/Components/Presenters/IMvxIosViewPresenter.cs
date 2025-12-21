namespace MvvmCross.Platforms.Ios.Presenters
{
    using MvvmCross.Platforms.Ios.Views;
    using Nivaes.App.Cross;

    public interface IMvxIosViewPresenter 
        : ICrossViewPresenter, IMvxCanCreateIosView
    {
        public void ClosedPopoverViewController();
    }
}
