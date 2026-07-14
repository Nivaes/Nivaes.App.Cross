namespace Nivaes.App.Cross.UIKitLib;

[Obsolete("", true)]
public sealed class MvxModalPresentationControllerDelegate(
        IosViewPresenterManager presenter,
        UIViewController viewController,
        ModalPresentationAttribute attribute)
    : UIAdaptivePresentationControllerDelegate
{
    public override void DidDismiss(UIPresentationController presentationController)
    {
        _ = presenter.CloseModalViewController(viewController, attribute);
    }
}
