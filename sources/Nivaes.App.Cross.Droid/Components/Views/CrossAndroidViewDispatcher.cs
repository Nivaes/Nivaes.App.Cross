// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;
using MvvmCross.Platforms.Android.Presenters;
using MvvmCross.ViewModels;
using MvvmCross.Views;

namespace Nivaes.App.Cross.Droid
{
    public class CrossAndroidViewDispatcher
        : CrossAndroidMainThreadDispatcher
        , IMvxViewDispatcher
    {
        private readonly IMvxAndroidViewPresenter _presenter;

        public CrossAndroidViewDispatcher(IMvxAndroidViewPresenter presenter)
        {
            _presenter = presenter;
        }

        public async Task<bool> ShowViewModel(CrossViewModelRequest request)
        {
            await ExecuteOnMainThreadAsync(() => _presenter.Show(request));
            return true;
        }

        public async Task<bool> ChangePresentation(MvxPresentationHint hint)
        {
            await ExecuteOnMainThreadAsync(() => _presenter.ChangePresentation(hint));
            return true;
        }
    }
}
