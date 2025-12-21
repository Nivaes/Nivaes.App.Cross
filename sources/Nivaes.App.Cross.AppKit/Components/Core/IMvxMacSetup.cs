namespace MvvmCross.Platforms.Mac.Core
{
    using MvvmCross.Platforms.Mac.Presenters;
    using Nivaes.App.Cross;

    public interface IMvxMacSetup 
        : ICrossSetup
    {
        void PlatformInitialize(IMvxApplicationDelegate applicationDelegate);
        void PlatformInitialize(IMvxApplicationDelegate applicationDelegate, IMvxMacViewPresenter presenter);
    }
}
