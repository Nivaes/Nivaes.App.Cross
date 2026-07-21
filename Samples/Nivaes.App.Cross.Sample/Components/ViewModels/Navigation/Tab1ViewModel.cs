using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross.Sample;

public class Tab1ViewModel
    : CrossNavigationViewModel<string>
{
    public Tab1ViewModel(ILogger<Tab1ViewModel> logger, CrossNavigationService navigationService)
        : base(navigationService, logger)
    {
        OpenChildCommand = new CrossAsyncCommand(() => NavigationService.Navigate<ChildViewModel>());

        OpenModalCommand = new CrossAsyncCommand(() => NavigationService.Navigate<ModalViewModel>());

        OpenNavModalCommand = new CrossAsyncCommand(() => NavigationService.Navigate<ModalNavViewModel>());

        CloseCommand = new CrossAsyncCommand(() => NavigationService.Close(this));

        OpenTab2Command = new CrossAsyncCommand(() => NavigationService.ChangePresentation(new CrossPagePresentationHint(typeof(Tab2ViewModel))));
    }

    public override Task Initialize()
    {
        return Task.Delay(3000);
    }

    private string? _parameter;

    public override void Prepare(string parameter)
    {
        _parameter = parameter;
    }

    public ICrossAsyncCommand OpenChildCommand { get; }

    public ICrossAsyncCommand OpenModalCommand { get; }

    public ICrossAsyncCommand OpenNavModalCommand { get; }

    public ICrossAsyncCommand OpenTab2Command { get; }

    public ICrossAsyncCommand CloseCommand { get; }
}
