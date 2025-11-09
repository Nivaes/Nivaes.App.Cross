namespace Nivaes.App.Cross.Droid
{
    public class CrossAndroidViewDispatcher
        : CrossAndroidMainThreadDispatcher
        , ICrossViewDispatcher
    {
        private readonly ICrossAndroidViewPresenter _presenter;

        public CrossAndroidViewDispatcher(ICrossAndroidViewPresenter presenter)
        {
            _presenter = presenter;
        }

        public async Task<bool> ShowViewModel(ICrossViewModelRequest request)
        {
            await ExecuteOnMainThreadAsync(() => _presenter.Show(request));
            return true;
        }

        public async Task<bool> ChangePresentation(CrossPresentationHint hint)
        {
            throw new NotImplementedException();
            //await ExecuteOnMainThreadAsync(() => _presenter.ChangePresentation(hint));
            //return true;
        }

        public Task<bool> ShowViewModelOnMainThread(ICrossViewModelRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ShowViewModelOnBackgroundThread(ICrossViewModelRequest request, Func<ICrossViewModelRequest, Task<bool>> action)
        {
            throw new NotImplementedException();
        }
    }
}
