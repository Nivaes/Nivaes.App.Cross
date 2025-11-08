using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross.UIKit
{
    public class CrossIosViewDispatcher
        : CrossIosUIThreadDispatcher, 
        ICrossViewDispatcher
    {
        private readonly ICrossIosViewPresenter _presenter;

        public CrossIosViewDispatcher(ICrossIosViewPresenter presenter)
        {
            _presenter = presenter;
        }

        public async Task<bool> ShowViewModel(CrossViewModelRequest request)
        {
            Task action()
            {
                CrossLogHost.GetLog<CrossIosViewDispatcher>()?.LogTrace(
                    "Navigate requested to {ViewModelType}", request?.ViewModelType);
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
