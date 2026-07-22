using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross.Droid
{
    // ToDo unificar  MvxAndroidViewDispatcher y MvxAndroidMainThreadDispatcher
    public sealed class MvxAndroidViewDispatcher
        : MvxAndroidMainThreadDispatcher
        , ICrossViewDispatcher
    {
        private readonly IAndroidViewPresenterManager _presenter;

        public MvxAndroidViewDispatcher(IAndroidViewPresenterManager presenter, ILogger<MvxAndroidViewDispatcher> logger)
            : base(logger)
        {
            _presenter = presenter;
        }

        public async Task<bool> ShowViewModel(ViewModelRequest request)
        {
            await ExecuteOnMainThreadAsync(() => _presenter.Show(request));
            return true;
        }

        public async Task<bool> ChangePresentation(CrossPresentationHint hint)
        {
            await ExecuteOnMainThreadAsync(() => _presenter.ChangePresentation(hint));
            return true;
        }

        //public ValueTask<bool> ShowViewModel(CrossViewModelRequest request)
        //{
        //    return ExecuteOnMainThreadAsync(() => _presenter.Show(request));
        //}

        //public ValueTask<bool> ChangePresentation(CrossPresentationHint hint)
        //{
        //    return ExecuteOnMainThreadAsync(() => _presenter.ChangePresentation(hint));
        //}
    }
}
