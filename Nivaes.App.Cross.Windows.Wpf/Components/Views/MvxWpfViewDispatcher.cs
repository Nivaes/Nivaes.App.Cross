// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.Platforms.Wpf.Views
{
    using System.Threading.Tasks;
    using System.Windows.Threading;
    using MvvmCross.Platforms.Wpf.Presenters;
    using Nivaes.App.Cross.ViewModels;
    using MvvmCross.Views;

    public class MvxWpfViewDispatcher
        : MvxWpfUIThreadDispatcher, IMvxViewDispatcher
    {
        private readonly IMvxWpfViewPresenter _presenter;

        public MvxWpfViewDispatcher(Dispatcher dispatcher, IMvxWpfViewPresenter presenter)
            : base(dispatcher)
        {
            _presenter = presenter;
        }

        public async ValueTask<bool> ShowViewModel(MvxViewModelRequest request)
        {
            await ExecuteOnMainThreadAsync(async() => await _presenter.Show(request).ConfigureAwait(false)).ConfigureAwait(false);
            return true;
        }

        public async ValueTask<bool> ChangePresentation(MvxPresentationHint hint)
        {
            await ExecuteOnMainThreadAsync(async () => await _presenter.ChangePresentation(hint).ConfigureAwait(false)).ConfigureAwait(false);
            return true;
        }
    }
}
