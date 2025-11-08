namespace Nivaes.App.Cross.Droid
{
    using Android.App;

    public interface ICrossAndroidSetup 
        : ICrossSetup
    {
        void PlatformInitialize(Application application);
    }
}
