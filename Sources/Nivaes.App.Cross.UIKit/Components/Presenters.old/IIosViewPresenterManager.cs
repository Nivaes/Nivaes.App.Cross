namespace Nivaes.App.Cross.UIKitLib
{
    public interface IIosViewPresenterManager
        : ICrossViewPresenterManager, IMvxCanCreateIosView
    {
#if IOS || MACCATALYST
        public void ClosedPopoverViewController();
#endif
    }
}
