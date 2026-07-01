namespace Nivaes.App.Cross.AppKitOS
{
    public interface ICrossViewController
    {
        NSViewController[] PresentedViewControllers { get; }
        void DismissViewController(NSViewController viewController);
    }
}
