namespace Nivaes.App.Cross.UIKit
{
    public interface ICrossIosViewPresenter 
        : ICrossViewPresenter, ICrossCanCreateIosView
    {
        public void ClosedPopoverViewController();
    }
}
