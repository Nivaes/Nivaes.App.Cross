namespace Nivaes.App.Cross.UIKitLib
{
    public interface IMvxIosViewPresenter
        : ICrossViewPresenter, IMvxCanCreateIosView
    {
#if IOS || MACCATALYST
        public void ClosedPopoverViewController();
#endif
    }
}
