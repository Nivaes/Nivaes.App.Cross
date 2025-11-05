namespace Nivaes.App.Cross.WinUI
{
    using System;
    using System.Threading.Tasks;

    public class CrossWindowsViewDispatcher
        : CrossWindowsMainThreadDispatcher, ICrossViewDispatcher
    {
        //private readonly ICrossWindowsViewPresenter _presenter;

        //public CrossWindowsViewDispatcher(ICrossWindowsViewPresenter presenter, ICrossWindowsFrame rootFrame)
        //    : base(rootFrame.UnderlyingControl.DispatcherQueue)
        //{
        //    _presenter = presenter;
        //}

        //public async Task<bool> ShowViewModel(CrossViewModelRequest request)
        //{
        //    await ExecuteOnMainThreadAsync(() => _presenter.Show(request));
        //    return true;
        //}

        //public async Task<bool> ChangePresentation(MvxPresentationHint hint)
        //{
        //    await ExecuteOnMainThreadAsync(() => _presenter.ChangePresentation(hint));
        //    return true;
        //}
        public Task<bool> ChangePresentation(CrossPresentationHint hint)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ShowViewModel(ICrossViewModelRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ShowViewModelOnBackgroundThread(ICrossViewModelRequest request, Func<ICrossViewModelRequest, Task<bool>> action)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ShowViewModelOnMainThread(ICrossViewModelRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
