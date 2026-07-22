using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross.UIKitLib
{
    public class MvxIosViewDispatcher
        : MvxIosUIThreadDispatcher, ICrossViewDispatcher
    {
        private readonly IIosViewPresenterManager _presenter;

        public MvxIosViewDispatcher(IIosViewPresenterManager presenter, ILogger<MvxIosViewDispatcher> logger)
            : base(logger)
        {
            _presenter = presenter;
        }

        public async Task<bool> ShowViewModel(IViewModelRequest request)
        {
            Task action()
            {
                Logger.LogTrace("Navigate requested to {ViewModelType}", request?.ViewModelType);

                return _presenter.Show(request!).AsTask();
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
