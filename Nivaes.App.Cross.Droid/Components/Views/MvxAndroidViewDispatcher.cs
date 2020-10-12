// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.Platforms.Android.Views
{
    using System.Threading.Tasks;
    using MvvmCross.ViewModels;
    using MvvmCross.Views;
    using Nivaes.App.Cross.Presenters;

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
            await ExecuteOnMainThreadAsync(() =>
            {
                return mPresenter.Show(request);
            }).ConfigureAwait(false);

            return true;
        }

        public async ValueTask<bool> ChangePresentation(MvxPresentationHint hint)
        {
            await ExecuteOnMainThreadAsync(() =>
            {
                return mPresenter.ChangePresentation(hint);
            }).ConfigureAwait(false);

            return true;
        }
    }
}
