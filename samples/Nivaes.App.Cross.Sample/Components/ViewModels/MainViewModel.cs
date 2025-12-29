using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross.Sample;

public class MainViewModel : CrossNavigationViewModel
{
    private string _bindableText = "I'm bound!";

    private int _counter = 2;

    public MainViewModel(ILoggerFactory logProvider, ICrossNavigationService navigationService)
        : base(logProvider, navigationService)
    {
        ShowChildCommand = new CrossAsyncCommand(() => NavigationService.Navigate<ChildViewModel>());

        ShowModalCommand = new CrossAsyncCommand(() => NavigationService.Navigate<ModalViewModel>());

        ShowModalNavCommand =
            new CrossAsyncCommand(() => NavigationService.Navigate<ModalNavViewModel>());

        ShowTabsCommand = new CrossAsyncCommand(() => NavigationService.Navigate<TabsRootViewModel>());

        ShowSplitCommand = new CrossAsyncCommand(() => NavigationService.Navigate<SplitRootViewModel>());

        ShowOverrideAttributeCommand = new CrossAsyncCommand(() => NavigationService.Navigate<OverrideAttributeViewModel>());

        ShowSheetCommand = new CrossAsyncCommand(() => NavigationService.Navigate<SheetViewModel>());

        ShowWindowCommand = new CrossAsyncCommand(() => NavigationService.Navigate<WindowViewModel>());

        ShowMixedNavigationCommand =
            new CrossAsyncCommand(() => NavigationService.Navigate<MixedNavFirstViewModel>());

        ShowCustomBindingCommand =
            new CrossAsyncCommand(() => NavigationService.Navigate<CustomBindingViewModel>());

        _counter = 3;
    }

    public ICrossAsyncCommand ShowChildCommand { get; }

    public ICrossAsyncCommand ShowModalCommand { get; }

    public ICrossAsyncCommand ShowModalNavCommand { get; }

    public ICrossAsyncCommand ShowTabsCommand { get; }

    public ICrossAsyncCommand ShowCustomBindingCommand { get; }

    public ICrossAsyncCommand ShowSplitCommand { get; }

    public ICrossAsyncCommand ShowOverrideAttributeCommand { get; }

    public ICrossAsyncCommand ShowSheetCommand { get; }

    public ICrossAsyncCommand ShowWindowCommand { get; }

    public ICrossAsyncCommand ShowMixedNavigationCommand { get; }

    public ICrossLanguageBinder TextSource => new CrossLanguageBinder("MvxBindingsExample", "Text");

    public string BindableText
    {
        get => _bindableText;
        set
        {
            SetProperty(ref _bindableText, value);
        }
    }

    protected override void SaveStateToBundle(ICrossBundle bundle)
    {
        base.SaveStateToBundle(bundle);

        bundle.Data["MyKey"] = _counter.ToString();
    }

    protected override void ReloadFromBundle(ICrossBundle state)
    {
        base.ReloadFromBundle(state);

        _counter = int.Parse(state.Data["MyKey"]);
    }
}
