namespace Nivaes.App.Cross.Droid
{
    public class MvxSingleViewModelCache
        : IMvxSingleViewModelCache
    {
        private const string BundleCacheKey = "ViewModelCacheKey";

        private int _counter;

        private WeakReference<ICrossViewModel>? _currentViewModel;

        public void Cache(ICrossViewModel toCache, Bundle bundle)
        {
            _currentViewModel = new WeakReference<ICrossViewModel>(toCache);
            _counter++;

            bundle.PutInt(BundleCacheKey, _counter);
        }

        public ICrossViewModel? GetAndClear(Bundle? bundle)
        {
            try
            {
                if (bundle == null)
                    return null;

                if (_currentViewModel?.TryGetTarget(out var storedViewModel) == true)
                {
                    var key = bundle.GetInt(BundleCacheKey);
                    var toReturn = key == _counter ? storedViewModel : null;
                    return toReturn;
                }
            }
            finally
            {
                _currentViewModel = null;
            }

            return null;
        }
    }
}
