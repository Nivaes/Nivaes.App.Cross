namespace MvvmCross.Platforms.Tvos.Views
{
    using MvvmCross.Platforms.Tvos.Presenters;
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross;
    using Nivaes.App.Cross.UIKit.TvOS;

    public class MvxTvosViewDispatcher
        : MvxTvosUIThreadDispatcher, ICrossViewDispatcher
    {
        private readonly ICrossTvosViewPresenter _presenter;

        public MvxTvosViewDispatcher(ICrossTvosViewPresenter presenter)
        {
            _presenter = presenter;
        }

        public async Task<bool> ShowViewModel(CrossViewModelRequest request)
        {
            Task action()
            {
                return _presenter.Show(request);
            }
            await ExecuteOnMainThreadAsync(action);
            return true;
        }

        public async Task<bool> ChangePresentation(CrossPresentationHint hint)
        {
            await ExecuteOnMainThreadAsync(() => _presenter.ChangePresentation(hint));
            return true;
        }
    }
}
