namespace Playground.Core.ViewModels
{
    using Microsoft.Extensions.Logging;
    using Nivaes.App.Cross;

    public class MixedNavResultDetailViewModel : CrossNavigationViewModel
    {
        public MixedNavResultDetailViewModel(ILogger<MixedNavResultDetailViewModel> logger, ICrossNavigationService navigationService)
            : base(navigationService, logger)
        {
            CloseViewModelCommand = new CrossAsyncCommand(() => NavigationService.Close(this));
        }

        public ICrossAsyncCommand CloseViewModelCommand { get; }
    }
}
