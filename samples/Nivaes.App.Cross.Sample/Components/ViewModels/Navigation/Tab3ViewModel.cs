using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross.Sample;

public class Tab3ViewModel 
    : CrossNavigationViewModel
{
    public Tab3ViewModel(ILoggerFactory logProvider, ICrossNavigationService navigationService)
        : base(logProvider, navigationService)
    {
        ShowRootViewModelCommand = new CrossAsyncCommand(() => NavigationService.Navigate<RootViewModel>());

        CloseViewModelCommand = new CrossAsyncCommand(() => NavigationService.Close(this));

        ShowPageOneCommand = new CrossCommand(() => NavigationService.ChangePresentation(new CrossPagePresentationHint(typeof(Tab1ViewModel))));
    }

    public ICrossAsyncCommand ShowRootViewModelCommand { get; }

    public ICrossAsyncCommand CloseViewModelCommand { get; }

    public ICrossCommand ShowPageOneCommand { get; }
}
