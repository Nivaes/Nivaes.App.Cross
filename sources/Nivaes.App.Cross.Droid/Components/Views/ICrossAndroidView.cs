// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Android.Binding.Views;
using MvvmCross.Platforms.Android.Views.Base;
using MvvmCross.ViewModels;
using MvvmCross.Views;

namespace Nivaes.App.Cross.Droid
{
    public interface ICrossAndroidView
        : IMvxView
        , ICrossLayoutInflaterHolder
        , ICrossStartActivityForResult
        , ICrossBindingContextOwner
    {
    }

    public interface IMvxAndroidView<TViewModel>
        : ICrossAndroidView
        , IMvxView<TViewModel> where TViewModel : class, ICrossViewModel
    {
        MvxFluentBindingDescriptionSet<IMvxAndroidView<TViewModel>, TViewModel> CreateBindingSet();
    }
}
