// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross.Presenters
{
    using System;
    using System.Collections.Generic;
    using MvvmCross.ViewModels;
    using MvvmCross.Views;

    public interface IMvxAttributeViewPresenter
        : IMvxViewPresenter
    {
        IMvxViewModelTypeFinder ViewModelTypeFinder { get; set; }
        IMvxViewsContainer ViewsContainer { get; set; }
        IDictionary<Type, MvxPresentationAttributeAction> AttributeTypesToActionsDictionary { get; }
        void RegisterAttributeTypes();

        MvxBasePresentationAttribute GetPresentationAttribute(MvxViewModelRequest request);
        MvxBasePresentationAttribute CreatePresentationAttribute(Type viewModelType, Type viewType);
        MvxBasePresentationAttribute GetOverridePresentationAttribute(MvxViewModelRequest request, Type viewType);
    }
}
