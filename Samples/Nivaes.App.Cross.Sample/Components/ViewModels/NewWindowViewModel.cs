using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross.Sample;

public class NewWindowViewModel
    : CrossNavigationViewModel
{
    private string _welcomeText = "Default welcome";

    public NewWindowViewModel(ILogger<NewWindowViewModel> logger)
        : base(logger)
    {
    }

    public ICrossAsyncCommand ShowRegionCommand =>
        new CrossAsyncCommand(() => this.NavigationService.Navigate<RegionViewModel>(this));

    public string WelcomeText
    {
        get => _welcomeText;
        set => SetProperty(ref _welcomeText, value);
    }
}
