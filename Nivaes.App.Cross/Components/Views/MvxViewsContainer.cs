// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.Views
{
    using System;
    using System.Collections.Generic;
    using MvvmCross.ViewModels;

    public abstract class MvxViewsContainer
        : IMvxViewsContainer
    {
        private readonly Dictionary<Type, Type> mBindingMap = new Dictionary<Type, Type>();
        private readonly List<IMvxViewFinder> mSecondaryViewFinders;
        private IMvxViewFinder? mLastResortViewFinder;

        protected MvxViewsContainer()
        {
            mSecondaryViewFinders = new List<IMvxViewFinder>();
        }

        public void AddAll(IDictionary<Type, Type> lookup)
        {
            if (lookup == null) throw new ArgumentNullException(nameof(lookup));

            foreach (var pair in lookup)
            {
                Add(pair.Key, pair.Value);
            }
        }

        public void Add(Type viewModelType, Type viewType)
        {
            mBindingMap[viewModelType] = viewType;
        }

        public void Add<TViewModel, TView>()
            where TViewModel : IMvxViewModel
            where TView : IMvxView
        {
            Add(typeof(TViewModel), typeof(TView));
        }

        public Type GetViewType(Type viewModelType)
        {
            if (mBindingMap.TryGetValue(viewModelType, out Type binding))
            {
                return binding;
            }

            foreach (var viewFinder in mSecondaryViewFinders)
            {
                binding = viewFinder.GetViewType(viewModelType);
                if (binding != null)
                {
                    return binding;
                }
            }

            if (mLastResortViewFinder != null)
            {
                binding = mLastResortViewFinder.GetViewType(viewModelType);
                if (binding != null)
                {
                    return binding;
                }
            }

            throw new KeyNotFoundException("Could not find view for " + viewModelType);
        }

        public void AddSecondary(IMvxViewFinder finder)
        {
            mSecondaryViewFinders.Add(finder);
        }

        public void SetLastResort(IMvxViewFinder finder)
        {
            mLastResortViewFinder = finder;
        }
    }
}
