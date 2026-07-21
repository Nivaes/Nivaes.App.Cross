using Microsoft.Extensions.Logging;


namespace Nivaes.App.Cross.Sample;

public class SheetViewModel
    : CrossNavigationViewModel
{
    public SheetViewModel(ILogger<SheetViewModel> logger, CrossNavigationService navigationService)
        : base(navigationService, logger)
    {
        CloseCommand = new CrossAsyncCommand(CloseSheet);
    }

    public ICrossAsyncCommand CloseCommand { get; }

    private Task CloseSheet()
    {
        return NavigationService.Close(this);
    }
}
