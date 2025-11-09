namespace Nivaes.App.Cross.UIKit
{
    public interface ICrossIosSetup : ICrossSetup
    {
        void PlatformInitialize(ICrossLifetime lifetimeInstance, UIWindow window);
        void PlatformInitialize(ICrossLifetime lifetimeInstance, ICrossIosViewPresenter presenter);
    }
}
