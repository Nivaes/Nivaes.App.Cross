// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.Platforms.Tvos.Views
{
    using System;
    using System.Threading.Tasks;
    using Nivaes.App.Cross.Logging;
    using MvvmCross.Platforms.Tvos.Presenters;
    using MvvmCross.ViewModels;
    using MvvmCross.Views;

    public class MvxTvosViewDispatcher
        : MvxTvosUIThreadDispatcher, IMvxViewDispatcher
    {
        private readonly IMvxTvosViewPresenter mPresenter;

        public MvxTvosViewDispatcher(IMvxTvosViewPresenter presenter)
        {
            mPresenter = presenter;
        }

        public async ValueTask<bool> ShowViewModel(MvxViewModelRequest request)
        {
            Func<ValueTask<bool>> action = async () =>
                {
                    MvxLog.Instance?.Trace("tvOSNavigation", "Navigate requested");
                    return await mPresenter.Show(request).ConfigureAwait(false);
                };

            return await ExecuteOnMainThreadAsync(action).ConfigureAwait(false);
        }

        public async ValueTask<bool> ChangePresentation(MvxPresentationHint hint)
        {
            return await ExecuteOnMainThreadAsync(async () => await mPresenter.ChangePresentation(hint).ConfigureAwait(false)).ConfigureAwait(false);
        }
    }
}
