namespace MvvmCross.Platforms.Android.Views
{
    using System.Threading.Tasks;
    using MvvmCross.Platforms.Android.Presenters;
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross;

    public class MvxAndroidViewDispatcher
        : MvxAndroidMainThreadDispatcher
        , ICrossViewDispatcher
    {
        private readonly IMvxAndroidViewPresenter _presenter;

        public MvxAndroidViewDispatcher(IMvxAndroidViewPresenter presenter)
        {
            _presenter = presenter;
        }

        public async Task<bool> ShowViewModel(MvxViewModelRequest request)
        {
            await ExecuteOnMainThreadAsync(() => _presenter.Show(request));
            return true;
        }

        public async Task<bool> ChangePresentation(MvxPresentationHint hint)
        {
            await ExecuteOnMainThreadAsync(() => _presenter.ChangePresentation(hint));
            return true;
        }
    }
}
