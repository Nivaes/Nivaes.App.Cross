using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Playground.Core.ViewModels;

namespace Nivaes.App.Cross.Sample;

public class RootViewModel 
    : MvxNavigationResultAwaitingViewModel<SampleModel>
{
    private readonly ICrossViewModelLoader _mvxViewModelLoader;

    private int _counter = 2;

    private string _welcomeText = "Default welcome";

    public ICrossLanguageBinder TextSource
    {
        get { return new CrossLanguageBinder("Playground.Core", "Text"); }
    }

    public RootViewModel(
            ILogger<RootViewModel> logger,
            ICrossNavigationService navigationService,
            ICrossViewModelLoader mvxViewModelLoader,
            ICrossResultViewModelManager resultViewModelManager)
        : base(logger, navigationService, resultViewModelManager)
    {
        _mvxViewModelLoader = mvxViewModelLoader;

        ShowChildCommand = new CrossAsyncCommand(() =>
        {
            return NavigationService.Navigate<ChildViewModel>();
        });

        ShowModalCommand = new CrossAsyncCommand(Navigate);

        ShowModalNavCommand = new CrossAsyncCommand(() => NavigationService.Navigate<ModalNavViewModel>());

        ShowTabsCommand = new CrossAsyncCommand(() => NavigationService.Navigate<TabsRootViewModel>());

        ShowPagesCommand = new CrossAsyncCommand(() => NavigationService.Navigate<PagesRootViewModel>());

        ShowSplitCommand = new CrossAsyncCommand(() => NavigationService.Navigate<SplitRootViewModel>());

        ShowNativeCommand = new CrossAsyncCommand(() => NavigationService.Navigate<NativeViewModel>());

        ShowOverrideAttributeCommand = new CrossAsyncCommand(async () => await NavigationService.Navigate<OverrideAttributeViewModel>());

        ShowSheetCommand = new CrossAsyncCommand(() => NavigationService.Navigate<SheetViewModel>());

        ShowWindowCommand = new CrossAsyncCommand(() => NavigationService.Navigate<WindowViewModel>());

        ShowMixedNavigationCommand = new CrossAsyncCommand(() => NavigationService.Navigate<MixedNavFirstViewModel>());

        ShowDictionaryBindingCommand = new CrossAsyncCommand(async () => await NavigationService.Navigate<DictionaryBindingViewModel>());

        ShowCollectionViewCommand = new CrossAsyncCommand(() => NavigationService.Navigate<CollectionViewModel, CollectionViewParameter>(new CollectionViewParameter(50)));

        ShowSharedElementsCommand = new CrossAsyncCommand(async () => await NavigationService.Navigate<SharedElementRootChildViewModel>());

        ShowCustomBindingCommand = new CrossAsyncCommand(() => NavigationService.Navigate<CustomBindingViewModel>());

        ShowFluentBindingCommand = new CrossAsyncCommand(() => NavigationService.Navigate<FluentBindingViewModel>());

        RegisterAndResolveWithReflectionCommand = new CrossAsyncCommand(RegisterAndResolveWithReflection);
        RegisterAndResolveWithNoReflectionCommand = new CrossAsyncCommand(RegisterAndResolveWithNoReflection);

        ShowViewModelWithResult = new CrossAsyncCommand(DoShowChildWithResult);

        _counter = 3;

        TriggerVisibilityCommand = new CrossCommand(() => IsVisible = !IsVisible);

        FragmentCloseCommand = new CrossAsyncCommand(() => NavigationService.Navigate<FragmentCloseViewModel>());

        ShowBottomNavigationCommand = new CrossAsyncCommand(async () => await NavigationService.Navigate<MultiBackStackViewModel>());
    }

    private Task DoShowChildWithResult()
    {
        return NavigationService.NavigateRegisteringToResult<ChildWithResultViewModel, SampleModel, SampleModel>(this,
            ResultViewModelManager, new SampleModel("Hello from Root!", 1.337m));
    }

    public CrossNotifyTask? MyTask { get; set; }

    public ICrossAsyncCommand ShowChildCommand { get; }

    public ICrossAsyncCommand ShowModalCommand { get; }

    public ICrossAsyncCommand ShowModalNavCommand { get; }

    public ICrossAsyncCommand ShowCustomBindingCommand { get; }

    public ICrossAsyncCommand ShowTabsCommand { get; }

    public ICrossAsyncCommand ShowPagesCommand { get; }

    public ICrossAsyncCommand ShowSplitCommand { get; }

    public ICrossAsyncCommand ShowOverrideAttributeCommand { get; }

    public ICrossAsyncCommand ShowNativeCommand { get; }

    public ICrossAsyncCommand ShowSheetCommand { get; }

    public ICrossAsyncCommand ShowWindowCommand { get; }

    public ICrossAsyncCommand ShowMixedNavigationCommand { get; }

    public ICrossAsyncCommand ShowDictionaryBindingCommand { get; }

    public ICrossAsyncCommand ShowCollectionViewCommand { get; }

    public ICrossAsyncCommand ShowListViewCommand => new CrossAsyncCommand(() => NavigationService.Navigate<ListViewModel>());

    public ICrossAsyncCommand ShowBindingsViewCommand => new CrossAsyncCommand(() => NavigationService.Navigate<BindingsViewModel>());

    public ICrossAsyncCommand ShowCodeBehindViewCommand => new CrossAsyncCommand(() => NavigationService.Navigate<CodeBehindViewModel>());

    public ICrossAsyncCommand ShowNavigationCloseCommand => new CrossAsyncCommand(() => NavigationService.Navigate<NavigationCloseViewModel>());

    public ICrossAsyncCommand ShowContentViewCommand => new CrossAsyncCommand(() => NavigationService.Navigate<ParentContentViewModel>());

    public ICrossAsyncCommand ShowConvertersCommand => new CrossAsyncCommand(() => NavigationService.Navigate<ConvertersViewModel>());

    public ICrossAsyncCommand ShowNewWindowCommand => new CrossAsyncCommand(() => NavigationService.Navigate<NewWindowViewModel>());
    public ICrossAsyncCommand ShowRegionCommand => new CrossAsyncCommand(() => this.NavigationService.Navigate<RegionViewModel>(this));

    public ICrossAsyncCommand ShowSharedElementsCommand { get; }

    public ICrossAsyncCommand ShowFluentBindingCommand { get; }

    public ICrossAsyncCommand RegisterAndResolveWithReflectionCommand { get; }

    public ICrossAsyncCommand RegisterAndResolveWithNoReflectionCommand { get; }

    public ICrossCommand TriggerVisibilityCommand { get; }

    public ICrossCommand FragmentCloseCommand { get; }
    public ICrossAsyncCommand? ShowLocationCommand { get; }

    public CrossAsyncCommand ShowViewModelWithResult { get; set; }

    public ICrossCommand ShowBottomNavigationCommand { get; }

    private bool _isVisible;

    public bool IsVisible
    {
        get => _isVisible;
        set => SetProperty(ref _isVisible, value);
    }

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

    public string? TimeToRegister { get; set; }

    public string? TimeToResolve { get; set; }

    public string? TotalTime { get; set; }

    public override Task Initialize()
    {
        Logger.LogWarning("Testing log");

        return base.Initialize();
    }

    public override void ViewAppearing()
    {
        base.ViewAppearing();

        MyTask = CrossNotifyTask.Create(
            async () =>
            {
                await Task.Delay(300);

                WelcomeText = "Welcome to MvvmCross!";

                throw new Exception("Boom!");
            }, exception => { });
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

    private Task Navigate()
    {
        return NavigationService.Navigate<ModalViewModel>();
    }

    private async Task RegisterAndResolveWithReflection()
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        //Mvx.IoCProvider.RegisterTypesWithReflection();
        var registered = stopwatch.ElapsedTicks;
        //for (int i = 0; i < 20; i++)
        //{
        //    Mvx.IoCProvider.ResolveTypes();
        //}
        stopwatch.Stop();
        var total = stopwatch.ElapsedTicks;
        var resolved = total - registered;

        TimeToRegister = $"Time to register using reflection - {registered}";
        TimeToResolve = $"Time to resolve using reflection - {resolved}";
        TotalTime = $"Total time using reflection - {total}";
        await RaiseAllPropertiesChanged();
    }

    private async Task RegisterAndResolveWithNoReflection()
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        //Mvx.IoCProvider.RegisterTypesWithNoReflection();
        var registered = stopwatch.ElapsedTicks;
        //for (int i = 0; i < 20; i++)
        //{
        //    Mvx.IoCProvider.ResolveTypes();
        //}
        stopwatch.Stop();
        var total = stopwatch.ElapsedTicks;
        var resolved = total - registered;

        TimeToRegister = $"Time to register - NO reflection - {registered}";
        TimeToResolve = $"Time to resolve - NO reflection - {resolved}";
        TotalTime = $"Total time - NO reflection - {total}";
        await RaiseAllPropertiesChanged();
    }

    public override bool ResultSet(ICrossResultSettingViewModel<SampleModel> viewModel, SampleModel result)
    {
        Logger.LogInformation("Got Result {@Result} from {ViewModel}", result, viewModel.GetType().Name);
        return true;
    }
}
