namespace Nivaes.App.Cross.UIKitOS
{
    public interface IMvxIosSetup 
        : ICrossSetup
    {
        void PlatformInitialize(ICrossLifetime lifetimeInstance, UIWindow window);
        void PlatformInitialize(ICrossLifetime lifetimeInstance, IMvxIosViewPresenter presenter);
    }
}
