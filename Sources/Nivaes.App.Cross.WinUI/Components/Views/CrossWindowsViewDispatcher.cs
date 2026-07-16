using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross.WinUI;

public class CrossWindowsViewDispatcher
    : CrossWindowsMainThreadDispatcher, ICrossViewDispatcher
{
    private readonly IWindowsViewPresenterManager _presenter;

    public CrossWindowsViewDispatcher(IWindowsViewPresenterManager presenter, ICrossWindowsFrame rootFrame,
            ILogger<CrossWindowsViewDispatcher> logger)
        : base(rootFrame, logger)
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

    //public ValueTask<bool> ShowViewModel(CrossViewModelRequest request)
    //{
    //    return ExecuteOnMainThreadAsync(() => _presenter.Show(request));
    //}

    //public ValueTask<bool> ChangePresentation(CrossPresentationHint hint)
    //{
    //    return ExecuteOnMainThreadAsync(() => _presenter.ChangePresentation(hint));
    //}
}
