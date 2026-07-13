#if IOS || MACCATALYST
namespace Nivaes.App.Cross.UIKitLib
{
    public class MvxPopoverPresentationControllerDelegate
        : UIPopoverPresentationControllerDelegate
    {
        private readonly IIosViewPresenterManager _presenter;

        public MvxPopoverPresentationControllerDelegate(IIosViewPresenterManager presenter)
        {
            _presenter = presenter;
        }

        public override UIModalPresentationStyle GetAdaptivePresentationStyle(UIPresentationController forPresentationController)
        {
            return UIModalPresentationStyle.None;
        }

        public override UIModalPresentationStyle GetAdaptivePresentationStyle(UIPresentationController controller, UITraitCollection traitCollection)
        {
            return UIModalPresentationStyle.None;
        }

        public override void DidDismissPopover(UIPopoverPresentationController popoverPresentationController)
        {
            _presenter.ClosedPopoverViewController();
        }
    }
}
#endif