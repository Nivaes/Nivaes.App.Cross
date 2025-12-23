namespace Nivaes.App.Cross.Droid
{
    using System.Threading.Tasks;
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

        public async Task<bool> ShowViewModel(CrossViewModelRequest request)
        {
            await ExecuteOnMainThreadAsync(() => _presenter.Show(request));
            return true;
        }

        public async Task<bool> ChangePresentation(CrossPresentationHint hint)
        {
            await ExecuteOnMainThreadAsync(() => _presenter.ChangePresentation(hint));
            return true;
        }
    }
}
