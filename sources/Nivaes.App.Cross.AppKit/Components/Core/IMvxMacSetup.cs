namespace Nivaes.App.Cross.AppKit
{
    public interface IMvxMacSetup 
        : ICrossSetup
    {
        void PlatformInitialize(IMvxApplicationDelegate applicationDelegate);
        void PlatformInitialize(IMvxApplicationDelegate applicationDelegate, IMvxMacViewPresenter presenter);
    }
}
