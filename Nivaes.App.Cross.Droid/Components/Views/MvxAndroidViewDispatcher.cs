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

        public async ValueTask<bool> ShowViewModel(MvxViewModelRequest request)
        {
            await ExecuteOnMainThreadAsync(async () =>
            {
                await mPresenter.Show(request).ConfigureAwait(false);
            }).ConfigureAwait(false);

            return true;
        }

        public async ValueTask<bool> ChangePresentation(MvxPresentationHint hint)
        {
            await ExecuteOnMainThreadAsync(async () =>
            {
                await mPresenter.ChangePresentation(hint).ConfigureAwait(false);
            }).ConfigureAwait(false);

            return true;
        }
    }
}
