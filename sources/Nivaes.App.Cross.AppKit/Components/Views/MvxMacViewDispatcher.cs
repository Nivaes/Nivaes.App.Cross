namespace MvvmCross.Platforms.Mac.Views
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using MvvmCross.Logging;
    using MvvmCross.Platforms.Mac.Presenters;
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross;

    public class MvxMacViewDispatcher
        : MvxMacUIThreadDispatcher
        , ICrossViewDispatcher
    {
        private readonly IMvxMacViewPresenter _presenter;

        public MvxMacViewDispatcher(IMvxMacViewPresenter presenter)
        {
            _presenter = presenter;
        }

        public async Task<bool> ShowViewModel(CrossViewModelRequest request)
        {
            Func<Task> action = () =>
            {
                MvxLogHost.Default?.Log(LogLevel.Trace, "MacNavigation", "Navigate requested");
                return _presenter.Show(request);
            };
            await ExecuteOnMainThreadAsync(action);
            return true;
        }

        public async Task<bool> ChangePresentation(CrossPresentationHint hint)
        {
            Func<Task> action = () =>
            {
                MvxLogHost.Default?.Log(LogLevel.Trace, "MacNavigation", "Change presentation requested");
                return _presenter.ChangePresentation(hint);
            };
            await ExecuteOnMainThreadAsync(action);
            return true;
        }
    }
}
