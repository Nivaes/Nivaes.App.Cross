namespace Nivaes.App.Cross.WinUI3
{
    using System.Threading.Tasks;
    using MvvmCross.Platforms.WinUi.Views;

    public class MvxWindowsViewDispatcher
        : MvxWindowsMainThreadDispatcher, ICrossViewDispatcher
    {
        private readonly IMvxWindowsViewPresenter _presenter;

        public MvxWindowsViewDispatcher(IMvxWindowsViewPresenter presenter, IMvxWindowsFrame rootFrame)
            : base(rootFrame.UnderlyingControl.DispatcherQueue)
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
