using Microsoft.Extensions.Logging;
using Nivaes.App.Cross;


namespace Nivaes.App.Cross.Sample;

public class SheetViewModel 
    : CrossNavigationViewModel
{
    public SheetViewModel(ILogger<SheetViewModel> logger, ICrossNavigationService navigationService)
        : base(logger, navigationService)
    {
        CloseCommand = new CrossAsyncCommand(CloseSheet);
    }

    public ICrossAsyncCommand CloseCommand { get; }

    private Task CloseSheet()
    {
        return NavigationService.Close(this);
    }
}
