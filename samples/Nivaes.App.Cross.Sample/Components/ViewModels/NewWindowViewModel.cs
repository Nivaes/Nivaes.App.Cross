namespace Playground.Core.ViewModels
{
    using Microsoft.Extensions.Logging;
    using Nivaes.App.Cross;
    using Playground.Core.ViewModels.Navigation;

    public class NewWindowViewModel 
        : CrossNavigationViewModel
    {
        private string _welcomeText = "Default welcome";

        public NewWindowViewModel(ILoggerFactory logFactory, ICrossNavigationService navigationService) : base(logFactory, navigationService)
        {
        }

        public ICrossAsyncCommand ShowRegionCommand =>
            new CrossAsyncCommand(() => this.NavigationService.Navigate<RegionViewModel>(this));

        public string WelcomeText
        {
            get => _welcomeText;
            set
            {
                ShouldLogInpc(true);
                SetProperty(ref _welcomeText, value);
                ShouldLogInpc(false);
            }
        }
    }
}
