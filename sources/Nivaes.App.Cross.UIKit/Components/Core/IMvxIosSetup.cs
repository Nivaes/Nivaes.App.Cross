namespace MvvmCross.Platforms.Ios.Core
{
    using MvvmCross.Platforms.Ios.Presenters;
    using Nivaes.App.Cross;

    public interface IMvxIosSetup 
        : ICrossSetup
    {
        void PlatformInitialize(ICrossLifetime lifetimeInstance, UIWindow window);
        void PlatformInitialize(ICrossLifetime lifetimeInstance, IMvxIosViewPresenter presenter);
    }
}
