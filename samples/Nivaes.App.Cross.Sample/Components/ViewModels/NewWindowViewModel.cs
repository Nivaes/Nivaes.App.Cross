using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross.Sample;

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
