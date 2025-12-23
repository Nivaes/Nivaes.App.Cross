namespace Playground.Core.ViewModels
{
    using Microsoft.Extensions.Logging;
    using Nivaes.App.Cross;

    public class MixedNavResultDetailViewModel : CrossNavigationViewModel
    {
        public MixedNavResultDetailViewModel(ILoggerFactory logProvider, ICrossNavigationService navigationService)
            : base(logProvider, navigationService)
        {
            CloseViewModelCommand = new CrossAsyncCommand(() => NavigationService.Close(this));
        }

        public ICrossAsyncCommand CloseViewModelCommand { get; }
    }
}
