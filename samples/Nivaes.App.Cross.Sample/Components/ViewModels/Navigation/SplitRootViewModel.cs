namespace Playground.Core.ViewModels
{
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross;

    public class SplitRootViewModel 
        : CrossNavigationViewModel
    {
        public SplitRootViewModel(ILoggerFactory logProvider, ICrossNavigationService navigationService)
            : base(logProvider, navigationService)
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
