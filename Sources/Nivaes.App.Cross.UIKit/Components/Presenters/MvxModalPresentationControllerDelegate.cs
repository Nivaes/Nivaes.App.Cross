namespace Nivaes.App.Cross.UIKitLib;

public sealed class MvxModalPresentationControllerDelegate(
        IosViewPresenterManager presenter,
        UIViewController viewController,
        MvxModalPresentationAttribute attribute)
    : UIAdaptivePresentationControllerDelegate
{
    public override void DidDismiss(UIPresentationController presentationController)
    {
        _ = presenter.CloseModalViewController(viewController, attribute);
    }
}
