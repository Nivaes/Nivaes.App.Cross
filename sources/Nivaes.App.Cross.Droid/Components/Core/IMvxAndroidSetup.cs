namespace MvvmCross.Platforms.Android.Core
{
    using Nivaes.App.Cross;

    public interface IMvxAndroidSetup 
        : ICrossSetup
    {
        void PlatformInitialize(Application application);
    }
}