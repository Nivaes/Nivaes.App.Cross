// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.Platforms.Mac.Views
{
    using System;
    using System.Threading.Tasks;
    using MvvmCross.Platforms.Mac.Presenters;
    using MvvmCross.Views;
    using Nivaes.App.Cross.Logging;
    using Nivaes.App.Cross.ViewModels;

    public class MvxMacViewDispatcher
        : MvxMacUIThreadDispatcher
        , IMvxViewDispatcher
    {
        private readonly IMvxMacViewPresenter _presenter;

        public MvxMacViewDispatcher(IMvxMacViewPresenter presenter)
        {
            _presenter = presenter;
        }

        public ValueTask<bool> ShowViewModel(MvxViewModelRequest request)
        {
            Func<ValueTask<bool>> action = () =>
            {
                MvxLog.Instance?.Trace("MacNavigation", "Navigate requested");
                return _presenter.Show(request);
            };

            return ExecuteOnMainThreadAsync(action);
        }

        public ValueTask<bool> ChangePresentation(MvxPresentationHint hint)
        {
            Func<ValueTask<bool>> action = () =>
            {
                MvxLog.Instance?.Trace("MacNavigation", "Change presentation requested");
                return _presenter.ChangePresentation(hint);
            };

            return ExecuteOnMainThreadAsync(action);
        }
    }
}
