namespace MvvmCross.Platforms.Android.Views
{
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross;

    public interface IMvxSingleViewModelCache
    {
        void Cache(ICrossViewModel toCache, Bundle bundle);

        ICrossViewModel? GetAndClear(Bundle bundle);
    }
}
