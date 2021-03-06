﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.Platforms.Uap.Views
{
    using System.Threading.Tasks;
    using MvvmCross.Platforms.Uap.Presenters;
    using MvvmCross.Views;
    using Nivaes.App.Cross.ViewModels;

    public class MvxWindowsViewDispatcher
        : MvxWindowsMainThreadDispatcher, IMvxViewDispatcher
    {
        private readonly IMvxWindowsViewPresenter _presenter;

        public MvxWindowsViewDispatcher(IMvxWindowsViewPresenter presenter, IMvxWindowsFrame rootFrame)
            : base(rootFrame.UnderlyingControl.Dispatcher)
        {
            _presenter = presenter;
        }

        public async ValueTask<bool> ShowViewModel(MvxViewModelRequest request)
        {
            await ExecuteOnMainThreadAsync(() =>
            {
                return _presenter.Show(request);
            }).ConfigureAwait(false);

            return true;
        }

        public async ValueTask<bool> ChangePresentation(MvxPresentationHint hint)
        {
            await ExecuteOnMainThreadAsync(() =>
            {
                return _presenter.ChangePresentation(hint);
            }).ConfigureAwait(false);

            return true;
        }
    }
}
