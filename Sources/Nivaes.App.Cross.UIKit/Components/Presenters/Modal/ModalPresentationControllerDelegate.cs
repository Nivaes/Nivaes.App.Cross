namespace Nivaes.App.Cross.UIKitLib;

public sealed class ModalPresentationControllerDelegate(
        ModalUIKitPressenterAction presenterAction,
        UIViewController viewController,
        MvxModalPresentationAttribute attribute)
    : UIAdaptivePresentationControllerDelegate
{
    public override void DidDismiss(UIPresentationController presentationController)
    {
        _ = presenterAction.CloseModalViewController(viewController, attribute);
    }
}
