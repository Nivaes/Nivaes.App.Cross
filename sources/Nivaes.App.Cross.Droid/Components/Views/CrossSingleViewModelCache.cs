// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable
using MvvmCross.ViewModels;

namespace Nivaes.App.Cross.Droid
{
    public class CrossSingleViewModelCache
        : ICrossSingleViewModelCache
    {
        private const string BundleCacheKey = "__mvxVMCacheKey";

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
