// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Nivaes.App.Cross.ViewModels;
using MvvmCross.Views;

namespace MvvmCross.Platforms.Uap.Views
{
    public interface IMvxWindowsView
        : IMvxView
    {
        void ClearBackStack();
    }

    public interface IMvxWindowsView<TViewModel>
        : IMvxWindowsView
        , IMvxView<TViewModel> where TViewModel : class, IMvxViewModel
    {
    }
}
