namespace Nivaes.App.Cross.UIKitLib;

public sealed class ModalPresentationControllerDelegate(
        ModalPressenterAction presenterAction,
        UIViewController viewController,
        ModalPresentationAttribute attribute)
    : UIAdaptivePresentationControllerDelegate
{
    public override void DidDismiss(UIPresentationController presentationController)
    {
        _ = presenterAction.CloseModalViewController(viewController, attribute);
    }
}
