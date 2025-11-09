namespace Nivaes.App.Cross.UIKit
{
    using System;
    using Microsoft.Extensions.Logging;
    using MvvmCross.Platforms.Ios.Presenters;

    public class CrossIosViewDispatcher
        : CrossIosUIThreadDispatcher, 
        ICrossViewDispatcher
    {
        private readonly ICrossIosViewPresenter _presenter;

        public CrossIosViewDispatcher(ICrossIosViewPresenter presenter)
        {
            _presenter = presenter;
        }

        public async Task<bool> ShowViewModel(ICrossViewModelRequest request)
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
