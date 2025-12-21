namespace MvvmCross.Platforms.Tvos.Core
{
    using MvvmCross.Platforms.Tvos.Presenters;
    using Nivaes.App.Cross;

    public interface IMvxTvosSetup 
        : ICrossSetup
    {
        void PlatformInitialize(IMvxApplicationDelegate applicationDelegate, UIWindow window);
        void PlatformInitialize(IMvxApplicationDelegate applicationDelegate, IMvxTvosViewPresenter presenter);
    }
}
