using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross.Sample;

public class MultiBackStackViewModel(ILogger<MultiBackStackViewModel> logger, ICrossNavigationService navigationService)
    : CrossNavigationViewModel(navigationService, logger)
{
    private bool _initialNavigationDone = false;

    private async Task ShowInitialViewModelsExecute()
    {
        if (_initialNavigationDone)
            return;
        _initialNavigationDone = true;
        await NavigationService.Navigate<MultiBackStackTab1ViewModel>();
    }

    public override void ViewAppearing()
    {
        base.ViewAppearing();
        //we can only show tabs after our host view is created
        Task.Run(ShowInitialViewModelsExecute);
    }

    protected override void SaveStateToBundle(ICrossBundle bundle)
    {
        base.SaveStateToBundle(bundle);
        bundle.Data[nameof(_initialNavigationDone)] = _initialNavigationDone.ToString();
    }

    protected override void ReloadFromBundle(ICrossBundle state)
    {
        base.ReloadFromBundle(state);
        if (state.Data.TryGetValue(nameof(_initialNavigationDone), out string initDone))
            _ = bool.TryParse(initDone, out _initialNavigationDone);
    }
}

public class MultiBackStackTab1ViewModel(ILogger<MultiBackStackTab1ViewModel> logger, ICrossNavigationService navigationService)
    : CrossNavigationViewModel(navigationService, logger)
{
    public ICrossCommand GoDeeperCommand { get; init; } = new CrossAsyncCommand(async () => await navigationService.Navigate<MultiBackStackInnerViewModel>());
}

public class MultiBackStackTab2ViewModel : CrossViewModel;

public class MultiBackStackInnerViewModel : MvxNavigationViewModel<int>
{
    public ICrossCommand GoDeeperCommand { get; init; }
    public ICrossCommand CloseCommand { get; init; }

    private int _depth;
    public int Depth
    {
        get => _depth;
        set => SetProperty(ref _depth, value);
    }

    public MultiBackStackInnerViewModel(ILogger<MultiBackStackInnerViewModel> logFactory, ICrossNavigationService navigationService)
        : base(logFactory, navigationService)
    {
        GoDeeperCommand = new CrossAsyncCommand(async () => await NavigationService.Navigate<MultiBackStackInnerViewModel, int>(Depth + 1));
        CloseCommand = new CrossAsyncCommand(async () => await NavigationService.Close(this));
    }

    public override void Prepare()
    {
        base.Prepare();
        Depth = 1;
    }

    public override void Prepare(int parameter)
    {
        Depth = parameter;
    }

    protected override void SaveStateToBundle(ICrossBundle bundle)
    {
        base.SaveStateToBundle(bundle);
        bundle.Data[nameof(Depth)] = Depth.ToString();
    }

    protected override void ReloadFromBundle(ICrossBundle state)
    {
        base.ReloadFromBundle(state);
        if (state.Data.TryGetValue(nameof(Depth), out string ds))
            _ = int.TryParse(ds, out _depth);
    }
}