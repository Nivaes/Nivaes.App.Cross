﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.Views
{

    using System;
    using System.Collections.Generic;
    using Nivaes.App.Cross.ViewModels;

    public interface IMvxViewsContainer
        : IMvxViewFinder
    {
        void AddAll(IDictionary<Type, Type> viewModelViewLookup);

        void Add(Type viewModelType, Type viewType);

        void Add<TViewModel, TView>()
            where TViewModel : IMvxViewModel
            where TView : IMvxView;

        void AddSecondary(IMvxViewFinder finder);

        void SetLastResort(IMvxViewFinder finder);
    }
}
