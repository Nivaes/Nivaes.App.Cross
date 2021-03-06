﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.Platforms.Android.Views
{
    using System.Threading.Tasks;
    using MvvmCross.Views;
    using Nivaes.App.Cross.Presenters;
    using Nivaes.App.Cross.ViewModels;

    public class MvxAndroidViewDispatcher
        : MvxAndroidMainThreadDispatcher
        , IMvxViewDispatcher
    {
        private readonly IMvxAndroidViewPresenter mPresenter;

        public MvxAndroidViewDispatcher(IMvxAndroidViewPresenter presenter)
        {
            mPresenter = presenter;
        }

        public ValueTask<bool> ShowViewModel(MvxViewModelRequest request)
        {
            return ExecuteOnMainThreadAsync(() =>
            {
                return mPresenter.Show(request);
            });
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
