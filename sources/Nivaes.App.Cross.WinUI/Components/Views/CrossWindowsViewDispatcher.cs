using System.Threading.Tasks;

namespace Nivaes.App.Cross.WinUI3;

public class CrossWindowsViewDispatcher
    : CrossWindowsMainThreadDispatcher, ICrossViewDispatcher
{
    private readonly IMvxWindowsViewPresenter _presenter;

    public CrossWindowsViewDispatcher(IMvxWindowsViewPresenter presenter, ICrossWindowsFrame rootFrame)
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
