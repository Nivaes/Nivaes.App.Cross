namespace MvvmCross.Platforms.Android.Core
{
    using Nivaes.App.Cross;

    [Obsolete("", true)]
    public interface IMvxAndroidSetup
        : ICrossSetup
    {
        void PlatformInitialize(Application application);
    }
}