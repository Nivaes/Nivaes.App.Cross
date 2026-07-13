#if IOS || MACCATALYST
namespace Nivaes.App.Cross.UIKitLib
{
    public class PopoverPresentationControllerDelegate
        : UIPopoverPresentationControllerDelegate
    {
        private readonly PopoverUIKitPressenterAction _presenterAction;

        public PopoverPresentationControllerDelegate(PopoverUIKitPressenterAction presenter)
        {
            _presenterAction = presenter;
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
            _presenterAction.ClosedPopoverViewController();
        }
    }
}
#endif