namespace Nivaes.App.Cross.AppKitOS
{
    public interface IMvxMacSetup 
        : ICrossSetup
    {
        void PlatformInitialize(IMvxApplicationDelegate applicationDelegate);
        void PlatformInitialize(IMvxApplicationDelegate applicationDelegate, IMvxMacViewPresenter presenter);
    }
}
