using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross.Sample;

public class Tab2ViewModel
    : CrossNavigationViewModel
{
    public Tab2ViewModel(ILogger<Tab2ViewModel> logger)
        : base(logger)
    {
        ShowRootViewModelCommand = new CrossAsyncCommand(() => NavigationService.Navigate<RootViewModel>());

        CloseViewModelCommand = new CrossAsyncCommand(() => NavigationService.Close(this));
    }

    public ICrossAsyncCommand ShowRootViewModelCommand { get; }

    public ICrossAsyncCommand CloseViewModelCommand { get; }
}
