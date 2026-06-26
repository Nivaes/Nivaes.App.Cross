using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross.UIKitOS
{
    public class MvxIosViewDispatcher
        : MvxIosUIThreadDispatcher, ICrossViewDispatcher
    {
        private readonly IMvxIosViewPresenter _presenter;

        public MvxIosViewDispatcher(IMvxIosViewPresenter presenter, ILogger<MvxIosViewDispatcher> logger)
            :base(logger)
        {
            _presenter = presenter;
        }

        public async Task<bool> ShowViewModel(CrossViewModelRequest request)
        {
            Task action()
            {
                Logger.LogTrace("Navigate requested to {ViewModelType}", request?.ViewModelType);

                return _presenter.Show(request!);
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
