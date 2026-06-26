namespace Nivaes.App.Cross.UIKitOS
{
    public interface IMvxIosViewPresenter
        : ICrossViewPresenter, IMvxCanCreateIosView
    {
#if IOS || MACCATALYST
        public void ClosedPopoverViewController();
#endif
    }
}
