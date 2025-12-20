namespace MvvmCross.Platforms.Tvos.Views
{
    using MvvmCross.Platforms.Tvos.Presenters;
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross;

    public class MvxTvosViewDispatcher
        : MvxTvosUIThreadDispatcher, ICrossViewDispatcher
    {
        private readonly IMvxTvosViewPresenter _presenter;

        public MvxTvosViewDispatcher(IMvxTvosViewPresenter presenter)
        {
            _presenter = presenter;
        }

        public async Task<bool> ShowViewModel(MvxViewModelRequest request)
        {
            Task action()
            {
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
