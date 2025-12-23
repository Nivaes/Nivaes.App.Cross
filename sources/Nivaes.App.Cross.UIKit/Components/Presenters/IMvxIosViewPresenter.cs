namespace Nivaes.App.Cross.UIKit
{
    public interface IMvxIosViewPresenter 
        : ICrossViewPresenter, IMvxCanCreateIosView
    {
#if IOS || MACCATALYST
        public void ClosedPopoverViewController();
#endif
    }
}
