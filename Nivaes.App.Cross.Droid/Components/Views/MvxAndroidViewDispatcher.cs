// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.Platforms.Android.Views
{
    using System.Threading.Tasks;
    using MvvmCross.Platforms.Android.Presenters;
    using MvvmCross.ViewModels;
    using MvvmCross.Views;

    public class MvxAndroidViewDispatcher
        : MvxAndroidMainThreadDispatcher
        , IMvxViewDispatcher
    {
        private readonly IMvxAndroidViewPresenter mPresenter;

        public MvxAndroidViewDispatcher(IMvxAndroidViewPresenter presenter)
        {
            mPresenter = presenter;
        }

        public async Task<bool> ShowViewModel(MvxViewModelRequest request)
        {
            await ExecuteOnMainThreadAsync(() => mPresenter.Show(request)).ConfigureAwait(false);
            return true;
        }

        public async Task<bool> ChangePresentation(MvxPresentationHint hint)
        {
            await ExecuteOnMainThreadAsync(() => mPresenter.ChangePresentation(hint)).ConfigureAwait(false);
            return true;
        }
    }
}
