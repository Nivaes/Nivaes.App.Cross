namespace Playground.Core.ViewModels.Navigation
{
    using Microsoft.Extensions.Logging;
    using MvvmCross.Navigation;
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross;

    public class RegionViewModel : MvxNavigationViewModel
    {
        public RegionViewModel(ILoggerFactory logFactory, IMvxNavigationService navigationService) : base(logFactory, navigationService)
        {
        }

        public ICrossAsyncCommand CloseRegionCommand =>
            new CrossAsyncCommand(() => this.NavigationService.Close(this));
    }
}
