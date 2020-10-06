// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.Platforms.Tizen.Views
{
    using System;
    using System.Threading.Tasks;
    using Nivaes.App.Cross.Logging;
    using MvvmCross.Platforms.Tizen.Presenters;
    using MvvmCross.ViewModels;
    using MvvmCross.Views;

    public class MvxTizenViewDispatcher
        : MvxTizenMainThreadDispatcher, IMvxViewDispatcher
    {
        private readonly IMvxTizenViewPresenter _presenter;

        public MvxTizenViewDispatcher(IMvxTizenViewPresenter presenter)
        {
            _presenter = presenter;
        }

        public async ValueTask<bool> ShowViewModel(MvxViewModelRequest request)
        {
            Func<ValueTask> action = async () =>
            {
                MvxLog.Instance.Trace("TizenNavigation", "Navigate requested");
                await _presenter.Show(request).ConfigureAwait(true);
            };
            await ExecuteOnMainThreadAsync(action).ConfigureAwait(true);
            return true;
        }

        public async ValueTask<bool> ChangePresentation(MvxPresentationHint hint)
        {
            await ExecuteOnMainThreadAsync(async () => await _presenter.ChangePresentation(hint).ConfigureAwait(true)).ConfigureAwait(true);
            return true;
        }
    }
}
