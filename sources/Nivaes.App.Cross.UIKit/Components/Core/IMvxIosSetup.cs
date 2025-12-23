namespace Nivaes.App.Cross.UIKit
{
    public interface IMvxIosSetup 
        : ICrossSetup
    {
        void PlatformInitialize(ICrossLifetime lifetimeInstance, UIWindow window);
        void PlatformInitialize(ICrossLifetime lifetimeInstance, IMvxIosViewPresenter presenter);
    }
}
