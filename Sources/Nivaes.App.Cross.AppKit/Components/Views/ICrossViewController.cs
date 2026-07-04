namespace Nivaes.App.Cross.AppKitLib
{
    public interface ICrossViewController
    {
        NSViewController[] PresentedViewControllers { get; }
        void DismissViewController(NSViewController viewController);
    }
}
