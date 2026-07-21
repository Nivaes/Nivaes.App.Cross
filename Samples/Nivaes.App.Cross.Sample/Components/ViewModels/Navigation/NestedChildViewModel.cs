using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross.Sample;

public class NestedChildViewModel
    : CrossNavigationViewModel
{
    public NestedChildViewModel(ILogger<NestedChildViewModel> logger, CrossNavigationService navigationService)
        : base(navigationService, logger)
    {
        CloseCommand = new CrossAsyncCommand(() => NavigationService.Close(this));
        PopToChildCommand = new CrossAsyncCommand(() => NavigationService.ChangePresentation(new CrossPopPresentationHint(typeof(ChildViewModel))));
        PopToRootCommand = new CrossAsyncCommand(() => NavigationService.ChangePresentation(new CrossPopToRootPresentationHint()));
        RemoveCommand = new CrossAsyncCommand(() => NavigationService.ChangePresentation(new CrossRemovePresentationHint(typeof(SecondChildViewModel))));
    }

    public ICrossAsyncCommand CloseCommand { get; }

    public ICrossAsyncCommand PopToChildCommand { get; }

    public ICrossAsyncCommand PopToRootCommand { get; }

    public ICrossAsyncCommand RemoveCommand { get; }
}
