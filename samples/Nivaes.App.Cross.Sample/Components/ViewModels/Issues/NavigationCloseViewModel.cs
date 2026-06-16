using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Nivaes.IoC;

namespace Nivaes.App.Cross.Sample;

public class NavigationCloseViewModel 
    : CrossViewModel
{
    private readonly ICrossNavigationService _mvxNavigationService;

    public NavigationCloseViewModel(ICrossNavigationService mvxNavigationService)
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
        return _mvxNavigationService.Close(IPlatformApplication.Current!.Services.GetRequiredService<SecondChildViewModel>());
    }
}
