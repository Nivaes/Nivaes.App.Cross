namespace Nivaes.App.Cross.UIKitLib;

public sealed class MvxModalPresentationControllerDelegate(
        MvxIosViewPresenter presenter,
        UIViewController viewController,
        MvxModalPresentationAttribute attribute)
    : UIAdaptivePresentationControllerDelegate
{
    public override void DidDismiss(UIPresentationController presentationController)
    {
        _ = presenter.CloseModalViewController(viewController, attribute);
    }
}
