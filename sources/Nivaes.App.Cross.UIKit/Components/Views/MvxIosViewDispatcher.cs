namespace MvvmCross.Platforms.Ios.Views
{
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using MvvmCross.Logging;
    using MvvmCross.Platforms.Ios.Presenters;
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross;

    public class MvxIosViewDispatcher
        : MvxIosUIThreadDispatcher, ICrossViewDispatcher
    {
        private readonly IMvxIosViewPresenter _presenter;

        public MvxIosViewDispatcher(IMvxIosViewPresenter presenter)
        {
            _presenter = presenter;
        }

        public async Task<bool> ShowViewModel(CrossViewModelRequest request)
        {
            Task action()
            {
                MvxLogHost.GetLog<MvxIosViewDispatcher>()?.LogTrace(
                    "Navigate requested to {ViewModelType}", request?.ViewModelType);
                return _presenter.Show(request);
            }
            await ExecuteOnMainThreadAsync(action);
            return true;
        }

        public async Task<bool> ChangePresentation(MvxPresentationHint hint)
        {
            await ExecuteOnMainThreadAsync(() => _presenter.ChangePresentation(hint));
            return true;
        }
    }
}
