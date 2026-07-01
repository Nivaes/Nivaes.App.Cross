namespace Nivaes.App.Cross.Droid
{
    public interface IMvxSingleViewModelCache
    {
        void Cache(ICrossViewModel toCache, Bundle bundle);

        ICrossViewModel? GetAndClear(Bundle? bundle);
    }
}
