// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Concurrent;
using MvvmCross.ViewModels;

namespace Nivaes.App.Cross.Droid
{
    public class CrossMultipleViewModelCache
        : ICrossMultipleViewModelCache
    {
        private readonly Lazy<ConcurrentDictionary<CachedViewModelType, ICrossViewModel>> _lazyCurrentViewModels;

        public CrossMultipleViewModelCache()
        {
            _lazyCurrentViewModels =
                new Lazy<ConcurrentDictionary<CachedViewModelType, ICrossViewModel>>(
                    () => new ConcurrentDictionary<CachedViewModelType, ICrossViewModel>());
        }

        private ConcurrentDictionary<CachedViewModelType, ICrossViewModel> CurrentViewModels => _lazyCurrentViewModels.Value;

        public void Cache(ICrossViewModel toCache, string viewModelTag = "singleInstanceCache")
        {
            if (toCache == null) return;

            var type = toCache.GetType();

            var cachedViewModelType = new CachedViewModelType(type, viewModelTag);
            CurrentViewModels.AddOrUpdate(cachedViewModelType, toCache, (_, _) => toCache);
        }

        public ICrossViewModel GetAndClear(Type viewModelType, string viewModelTag = "singleInstanceCache")
        {
            if (viewModelType == null) return null;

            ICrossViewModel vm;
            var cachedViewModelType = new CachedViewModelType(viewModelType, viewModelTag);
            CurrentViewModels.TryRemove(cachedViewModelType, out vm);

            return vm;
        }

        public T GetAndClear<T>(string viewModelTag = "singleInstanceCache") where T : ICrossViewModel
        {
            return (T)GetAndClear(typeof(T), viewModelTag);
        }

        private class CachedViewModelType
        {
            public Type ViewModelType { get; }
            public string ViewModelTag { get; }

            public CachedViewModelType(Type viewModelType, string viewModelTag)
            {
                ViewModelType = viewModelType;
                ViewModelTag = viewModelTag ?? string.Empty;
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    int hashCode = 17;
                    hashCode = (hashCode * 23) + ViewModelType.GetHashCode();
                    hashCode = (hashCode * 23) + ViewModelTag.GetHashCode();
                    return hashCode;
                }
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(obj, this))
                    return true;

                var other = obj as CachedViewModelType;

                return other != null &&
                       other.ViewModelTag.Equals(ViewModelTag) &&
                       other.ViewModelType == ViewModelType;
            }
        }
    }
}
