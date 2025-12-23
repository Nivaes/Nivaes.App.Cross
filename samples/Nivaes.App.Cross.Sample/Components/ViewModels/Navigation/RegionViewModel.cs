namespace Playground.Core.ViewModels.Navigation
{
    using Microsoft.Extensions.Logging;
    using Nivaes.App.Cross;

    public class RegionViewModel 
        : CrossNavigationViewModel
    {
        public RegionViewModel(ILoggerFactory logFactory, ICrossNavigationService navigationService) : base(logFactory, navigationService)
        {
        }

        public ICrossAsyncCommand CloseRegionCommand =>
            new CrossAsyncCommand(() => this.NavigationService.Close(this));
    }
}
