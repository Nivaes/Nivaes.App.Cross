using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross.Sample;

public class NavigationCloseViewModel
    : CrossViewModel
{
    private readonly CrossNavigationService _mvxNavigationService;

    public NavigationCloseViewModel(CrossNavigationService mvxNavigationService, ILogger<NavigationCloseViewModel> logger)
        : base(logger)
    {
        _mvxNavigationService = mvxNavigationService;
    }

    public ICrossAsyncCommand OpenChildThenCloseThisCommand => new CrossAsyncCommand(CloseThisAndOpenChildAsync);

    public ICrossAsyncCommand TryToCloseNewViewModelCommand => new CrossAsyncCommand(TryToCloseNewViewModelAsync);

    private async Task CloseThisAndOpenChildAsync()
    {
        await _mvxNavigationService.Navigate<SecondChildViewModel>();
        await _mvxNavigationService.Close(this);
    }

    private Task TryToCloseNewViewModelAsync()
    {
        return _mvxNavigationService.Close(IPlatformApplication.Current!.ServiceProvider.GetRequiredService<SecondChildViewModel>());
    }
}
