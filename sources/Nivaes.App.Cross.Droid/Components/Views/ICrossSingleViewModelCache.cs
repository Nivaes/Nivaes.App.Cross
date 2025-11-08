namespace Nivaes.App.Cross.Droid
{
    public interface ICrossSingleViewModelCache
    {
        void Cache(ICrossViewModel toCache, Bundle bundle);

        ICrossViewModel? GetAndClear(Bundle bundle);
    }
}
