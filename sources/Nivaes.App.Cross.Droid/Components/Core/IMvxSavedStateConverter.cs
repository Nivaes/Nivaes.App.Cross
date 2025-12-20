using Android.OS;

namespace MvvmCross.Platforms.Android.Core
{
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross;

    public interface IMvxSavedStateConverter
    {
        ICrossBundle Read(Bundle bundle);

        void Write(Bundle bundle, ICrossBundle savedState);
    }
}
