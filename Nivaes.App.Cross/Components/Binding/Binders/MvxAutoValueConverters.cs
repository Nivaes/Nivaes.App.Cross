// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.Binding.Binders
{
    using System;
    using System.Collections.Generic;
    using MvvmCross.Converters;

    public class MvxAutoValueConverters
        : IMvxAutoValueConverters
    {
        public class Key
        {
            public Key(Type viewModel, Type view)
            {
                ViewType = view;
                ViewModelType = viewModel;
            }

            public Type ViewModelType { get; }
            public Type ViewType { get; }

            public override bool Equals(object obj)
            {
                if (!(obj is Key rhs))
                    return false;

                return ViewModelType == rhs.ViewModelType
                       && ViewType == rhs.ViewType;
            }

            public override int GetHashCode()
            {
                return ViewModelType.GetHashCode() + ViewType.GetHashCode();
            }
        }

        private readonly Dictionary<Key, IMvxValueConverter> _lookup = new Dictionary<Key, IMvxValueConverter>();

        public IMvxValueConverter Find(Type viewModelType, Type viewType)
        {
            IMvxValueConverter result;
            _lookup.TryGetValue(new Key(viewModelType, viewType), out result);
            return result;
        }

        public void Register(Type viewModelType, Type viewType, IMvxValueConverter converter)
        {
            _lookup[new Key(viewModelType, viewType)] = converter;
        }
    }
}
