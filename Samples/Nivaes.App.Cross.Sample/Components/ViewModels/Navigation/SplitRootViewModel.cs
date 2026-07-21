using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross.Sample
{
    public class SplitRootViewModel
        : CrossNavigationViewModel
    {
        public SplitRootViewModel(ILogger<SplitRootViewModel> logger)
            : base(logger)
        {
            ShowInitialMenuCommand = new CrossAsyncCommand(ShowInitialViewModel);
            ShowDetailCommand = new CrossAsyncCommand(ShowDetailViewModel);
        }

        public ICrossAsyncCommand ShowInitialMenuCommand { get; }

        public ICrossAsyncCommand ShowDetailCommand { get; }

        public override void ViewAppeared()
        {
            CrossNotifyTask.Create(async () =>
            {
                await ShowInitialViewModel();
                await ShowDetailViewModel();
            });
        }

        private Task ShowInitialViewModel()
        {
            return NavigationService.Navigate<SplitMasterViewModel>();
        }

        private Task ShowDetailViewModel()
        {
            return NavigationService.Navigate<SplitDetailViewModel>();
        }
    }
}
