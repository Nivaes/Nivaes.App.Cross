using Android.Content;

namespace Nivaes.App.Cross.Droid
{
    public interface IMvxAndroidViewModelLoader
    {
        ICrossViewModel? Load(Intent intent, ICrossBundle? savedState);

        ICrossViewModel? Load(
            Intent intent,
            ICrossBundle? savedState,
            Type? viewModelTypeHint);
    }
}