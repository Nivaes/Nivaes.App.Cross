using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross.Sample;

public class FragmentCloseViewModel : BaseViewModel
{
    private static int _counter = 0;

    public FragmentCloseViewModel(ILogger<FragmentCloseViewModel> logger, ICrossNavigationService navigationService)
        : base(logger, navigationService)
    {
        ForwardCommand = new CrossAsyncCommand(() => NavigationService.Navigate<FragmentCloseViewModel>());
        CloseCommand = new CrossAsyncCommand(() => NavigationService.Close(this));

        Description = $"View number {_counter++}";
    }

    private string? _description;
    public string? Description
    {
        get => _description;
        set => SetProperty(ref _description, value);
    }

    public ICrossAsyncCommand ForwardCommand { get; }
    public ICrossAsyncCommand CloseCommand { get; }
}
