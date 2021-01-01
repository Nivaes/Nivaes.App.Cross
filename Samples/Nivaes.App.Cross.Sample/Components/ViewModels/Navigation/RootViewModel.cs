// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross.Sample
{
    using System.Globalization;
    using System.Threading.Tasks;
    using MvvmCross.Localization;
    using Nivaes.App.Cross.Commands;
    using Nivaes.App.Cross.Logging;
    using Nivaes.App.Cross.Navigation;
    using Nivaes.App.Cross.ViewModels;

    public class RootViewModel
        : MvxNavigationViewModel
    {
        private readonly IMvxViewModelLoader mMvxViewModelLoader;

        private int mCounter = 2;

        public IMvxLanguageBinder TextSource => new MvxLanguageBinder("Playground.Core", "Text");

        public RootViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService, IMvxViewModelLoader mvxViewModelLoader)
            : base(logProvider, navigationService)
        {
            mMvxViewModelLoader = mvxViewModelLoader;
            //try
            //{
            //    var messenger = Mvx.IoCProvider.Resolve<IMvxMessenger>();
            //    var str = messenger.ToString();
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e.ToString());
            //}

            ShowChildCommand = new MvxCommandAsync(async () =>
            {
                _ = await NavigationService.Navigate<ChildViewModel, SampleModel, SampleModel>(
                    new SampleModel
                    {
                        Message = "Hey",
                        Value = 1.23m
                    }).ConfigureAwait(false);

                return true;
            });

            ShowModalCommand = new MvxCommandAsync(Navigate);

            ShowModalNavCommand = new MvxAsyncNavigationCommand<ModalNavViewModel>();

            ShowTabsCommand = new MvxAsyncNavigationCommand<TabsRootViewModel>();

            ShowPagesCommand = new MvxAsyncNavigationCommand<PagesRootViewModel>();

            ShowSplitCommand = new MvxAsyncNavigationCommand<SplitRootViewModel>();

            ShowNativeCommand = new MvxAsyncNavigationCommand<NativeViewModel>();

            ShowOverrideAttributeCommand = new MvxAsyncNavigationCommand<OverrideAttributeViewModel>();

            ShowSheetCommand = new MvxAsyncNavigationCommand<SheetViewModel>();

            ShowWindowCommand = new MvxAsyncNavigationCommand<WindowViewModel>();

            ShowMixedNavigationCommand = new MvxAsyncNavigationCommand<MixedNavFirstViewModel>();

            ShowDictionaryBindingCommand = new MvxAsyncNavigationCommand<DictionaryBindingViewModel>();

            ShowCollectionViewCommand = new MvxAsyncNavigationCommand<CollectionViewModel>();

            ShowSharedElementsCommand = new MvxAsyncNavigationCommand<SharedElementRootChildViewModel>();

            ShowCustomBindingCommand = new MvxAsyncNavigationCommand<CustomBindingViewModel>();

            ShowFluentBindingCommand = new MvxAsyncNavigationCommand<FluentBindingViewModel>();

            //RegisterAndResolveWithReflectionCommand = new MvxAsyncCommand(
            //    async () => await RegisterAndResolveWithReflection().ConfigureAwait(true));
            //RegisterAndResolveWithNoReflectionCommand = new MvxAsyncCommand(
            //    async () => await RegisterAndResolveWithNoReflection().ConfigureAwait(true));

            mCounter = 3;

            TriggerVisibilityCommand = new MvxCommand(() => IsVisible = !IsVisible);

            FragmentCloseCommand = new MvxAsyncNavigationCommand<FragmentCloseViewModel>();

            //ShowLocationCommand = new MvxAsyncCommand(
            //    async () => await NavigationService.Navigate<LocationViewModel>().ConfigureAwait(true));
        }

        public IMvxCommandAsync? ShowChildCommand { get; }

        public IMvxCommandAsync? ShowModalCommand { get; }

        public IMvxCommandAsync? ShowModalNavCommand { get; }

        public IMvxCommandAsync? ShowCustomBindingCommand { get; }

        public IMvxCommandAsync? ShowTabsCommand { get; }

        public IMvxCommandAsync? ShowPagesCommand { get; }

        public IMvxCommandAsync? ShowSplitCommand { get; }

        public IMvxCommandAsync? ShowOverrideAttributeCommand { get; }

        public IMvxCommandAsync? ShowNativeCommand { get; }

        public IMvxCommandAsync? ShowSheetCommand { get; }

        public IMvxCommandAsync? ShowWindowCommand { get; }

        public IMvxCommandAsync? ShowMixedNavigationCommand { get; }

        public IMvxCommandAsync? ShowDictionaryBindingCommand { get; }

        public IMvxCommandAsync? ShowCollectionViewCommand { get; }

        public IMvxCommandAsync ShowListViewCommand =>
            new MvxCommandAsync(async () => await NavigationService.Navigate<ListViewModel>().ConfigureAwait(true));

        public IMvxCommandAsync ShowBindingsViewCommand =>
            new MvxCommandAsync(async () => await NavigationService.Navigate<BindingsViewModel>().ConfigureAwait(true));

        public IMvxCommandAsync ShowCodeBehindViewCommand =>
            new MvxCommandAsync(async () => await NavigationService.Navigate<CodeBehindViewModel>().ConfigureAwait(true));

        public IMvxCommandAsync ShowNavigationCloseCommand =>
            new MvxCommandAsync(async () => await NavigationService.Navigate<NavigationCloseViewModel>().ConfigureAwait(true));

        public IMvxCommandAsync ShowContentViewCommand =>
            new MvxCommandAsync(async () => await NavigationService.Navigate<ParentContentViewModel>().ConfigureAwait(true));

        public IMvxCommandAsync ConvertersCommand =>
            new MvxCommandAsync(async () => await NavigationService.Navigate<ConvertersViewModel>().ConfigureAwait(true));

        public IMvxCommandAsync ShowSharedElementsCommand { get; }

        public IMvxCommandAsync ShowFluentBindingCommand { get; }

        public IMvxCommandAsync? RegisterAndResolveWithReflectionCommand { get; }

        public IMvxCommandAsync? RegisterAndResolveWithNoReflectionCommand { get; }

        public IMvxCommand TriggerVisibilityCommand { get; }

        public IMvxCommand FragmentCloseCommand { get; }
        public IMvxCommandAsync? ShowLocationCommand { get; }

        private bool mIsVisible;

        public bool IsVisible
        {
            get => mIsVisible;
            set => SetProperty(ref mIsVisible, value);
        }

        private string mWelcomeText = "Default welcome";

        public string WelcomeText
        {
            get => mWelcomeText;
            set
            {
                ShouldLogInpc(true);
                SetProperty(ref mWelcomeText, value);
                ShouldLogInpc(false);
            }
        }

        public string? TimeToRegister { get; set; }

        public string? TimeToResolve { get; set; }

        public string? TotalTime { get; set; }

        public override async ValueTask Initialize()
        {
            Log.Warn(() => "Testing log");

            await base.Initialize().ConfigureAwait(false);

            // Uncomment this to demonstrate use of StartAsync for async first navigation
            // await Task.Delay(5000);

            mMvxViewModelLoader.LoadViewModel(MvxViewModelRequest.GetDefaultRequest(typeof(ChildViewModel)),
                new SampleModel
                {
                    Message = "From locator",
                    Value = 2
                },
                null);

            //MakeRequest().ConfigureAwait(false);
        }

        public override ValueTask ViewAppearing()
        {
            return base.ViewAppearing();
        }

        protected override async ValueTask SaveStateToBundle(IMvxBundle? bundle)
        {
            await base.SaveStateToBundle(bundle).ConfigureAwait(false);

            if(bundle != null)
                bundle.Data["MyKey"] = mCounter.ToString(CultureInfo.CurrentCulture);
        }

        protected override async ValueTask ReloadFromBundle(IMvxBundle? bundle)
        {
            await base.ReloadFromBundle(bundle).ConfigureAwait(false);

            if (bundle != null)
                mCounter = int.Parse(bundle.Data["MyKey"], CultureInfo.CurrentCulture);
        }

        private ValueTask<bool> Navigate()
        {
            return NavigationService.Navigate<ModalViewModel>();
        }

        //public async Task<MvxRestResponse> MakeRequest()
        //{
        //    try
        //    {
        //        var request = new MvxRestRequest("http://github.com/asdsadadad");
        //        if (Mvx.IoCProvider.TryResolve(out IMvxRestClient client))
        //        {
        //            var task = client.MakeRequestAsync(request);

        //            var result = await task.ConfigureAwait(true);

        //            return result;
        //        }
        //    }
        //    catch (WebException webException)
        //    {
        //    }
        //    return default(MvxRestResponse);
        //}

        //private Task RegisterAndResolveWithReflection()
        //{
        //    var stopwatch = new Stopwatch();
        //    stopwatch.Start();
        //    Mvx.IoCProvider.RegisterTypesWithReflection();
        //    var registered = stopwatch.ElapsedTicks;
        //    for (int i = 0; i < 20; i++)
        //    {
        //        Mvx.IoCProvider.ResolveTypes();
        //    }
        //    stopwatch.Stop();
        //    var total = stopwatch.ElapsedTicks;
        //    var resolved = total - registered;

        //    TimeToRegister = $"Time to register using reflection - {registered}";
        //    TimeToResolve = $"Time to resolve using reflection - {resolved}";
        //    TotalTime = $"Total time using reflection - {total}";
        //    return RaiseAllPropertiesChanged();
        //}

        //private Task RegisterAndResolveWithNoReflection()
        //{
        //    var stopwatch = new Stopwatch();
        //    stopwatch.Start();
        //    Mvx.IoCProvider.RegisterTypesWithNoReflection();
        //    var registered = stopwatch.ElapsedTicks;
        //    for (int i = 0; i < 20; i++)
        //    {
        //        Mvx.IoCProvider.ResolveTypes();
        //    }
        //    stopwatch.Stop();
        //    var total = stopwatch.ElapsedTicks;
        //    var resolved = total - registered;

        //    TimeToRegister = $"Time to register - NO reflection - {registered}";
        //    TimeToResolve = $"Time to resolve - NO reflection - {resolved}";
        //    TotalTime = $"Total time - NO reflection - {total}";
        //    return RaiseAllPropertiesChanged();
        //}
    }
}
