#if IOS || MACCATALYST
namespace Nivaes.App.Cross.UIKitLib
{
    internal class MediaPickerPopoverDelegate : UIPopoverControllerDelegate
    {
        private readonly MediaPickerDelegate mPickerDelegate;
        private readonly UINavigationController mPicker;

        internal MediaPickerPopoverDelegate(MediaPickerDelegate pickerDelegate, UINavigationController picker)
        {
            mPickerDelegate = pickerDelegate;
            mPicker = picker;
        }

        public override bool ShouldDismiss(UIPopoverController popoverController) => true;

        public override void DidDismiss(UIPopoverController popoverController) => mPickerDelegate.Canceled(mPicker);
    }
}
#endif