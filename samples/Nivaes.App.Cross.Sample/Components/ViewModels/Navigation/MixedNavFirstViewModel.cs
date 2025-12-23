namespace Playground.Core.ViewModels
{
    using Microsoft.Extensions.Logging;
    using Nivaes.App.Cross;

    public class MixedNavFirstViewModel : CrossNavigationViewModel
    {
        public MixedNavFirstViewModel(ILoggerFactory logProvider, ICrossNavigationService navigationService)
            : base(logProvider, navigationService)
        {
        }

        public ICrossAsyncCommand LoginCommand => new CrossAsyncCommand(GotoMasterDetailPage, CanLogin);

        private bool CanLogin()
        {
            return true;
        }

        private async Task GotoMasterDetailPage()
        {
            await NavigationService.Navigate<MixedNavMasterDetailViewModel>();
            await NavigationService.Navigate<MixedNavMasterRootContentViewModel>();
        }
    }
}
