namespace Playground.Core.ViewModels
{
    using Microsoft.Extensions.Logging;
    using Nivaes.App.Cross;

    public class SheetViewModel 
        : CrossNavigationViewModel
    {
        public SheetViewModel(ILoggerFactory logProvider, ICrossNavigationService navigationService)
            : base(logProvider, navigationService)
        {
            CloseCommand = new CrossAsyncCommand(CloseSheet);
        }

        public ICrossAsyncCommand CloseCommand { get; }

        private Task CloseSheet()
        {
            return NavigationService.Close(this);
        }
    }
}
