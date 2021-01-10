// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.Platforms.Tizen.Views
{
    using System;
    using System.Threading.Tasks;
    using MvvmCross.Platforms.Tizen.Presenters;
    using MvvmCross.Views;
    using Nivaes.App.Cross.Logging;
    using Nivaes.App.Cross.ViewModels;

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
            Func<ValueTask<bool>> action = async () =>
            {
                //MvxLog.Instance.Trace("TizenNavigation", "Navigate requested");
                return await _presenter.Show(request).ConfigureAwait(true);
            };
            return await ExecuteOnMainThreadAsync(action).ConfigureAwait(true);
        }

        public async ValueTask<bool> ChangePresentation(MvxPresentationHint hint)
        {
            return await ExecuteOnMainThreadAsync(async () => await _presenter.ChangePresentation(hint).ConfigureAwait(true)).ConfigureAwait(true);
        }
    }
}
