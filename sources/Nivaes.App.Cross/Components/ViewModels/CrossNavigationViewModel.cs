namespace Nivaes.App.Cross
{
    using System.Diagnostics;
    using Microsoft.Extensions.Logging;
    using OpenTelemetry.Resources;
    using OpenTelemetry.Trace;

    public abstract class CrossNavigationViewModel
        : CrossViewModel
    {
        private ILogger? _logger;

        private static readonly ActivitySource Source = new("SampleCrossClient");

        protected CrossNavigationViewModel(ILoggerFactory logFactory, ICrossNavigationService navigationService)
        {
            LoggerFactory = logFactory;
            NavigationService = navigationService;

            Log.LogCritical($"Se inicio {this.GetType().Name}");

            using var activity = Source.StartActivity("Test");

            activity?.SetTag("Prueba", this.GetType().Name);

            //await Task.Delay(1000);
            Thread.Sleep(1000);

            activity?.Stop();
        }

        protected virtual ICrossNavigationService NavigationService { get; }

        protected virtual ILoggerFactory LoggerFactory { get; }

        protected virtual ILogger Log => _logger ??= LoggerFactory.CreateLogger(GetType().Name);
    }

    public abstract class MvxNavigationViewModel<TParameter>
        : CrossNavigationViewModel, ICrossViewModel<TParameter>
    {
        protected MvxNavigationViewModel(ILoggerFactory logFactory, ICrossNavigationService navigationService)
            : base(logFactory, navigationService)
        {
            Log.LogCritical($"Se inicio {this.GetType().Name}");
        }

        public abstract void Prepare(TParameter parameter);
    }

    public abstract class MvxNavigationResultAwaitingViewModel<TResult>
        : CrossNavigationViewModel, ICrossResultAwaitingViewModel<TResult>
    {
        protected ICrossResultViewModelManager ResultViewModelManager { get; }

        protected MvxNavigationResultAwaitingViewModel(
                ILoggerFactory logFactory,
                ICrossNavigationService navigationService,
                ICrossResultViewModelManager resultViewModelManager)
            : base(logFactory, navigationService)
        {
            ResultViewModelManager = resultViewModelManager;
        }

        protected override void ReloadFromBundle(ICrossBundle state)
        {
            base.ReloadFromBundle(state);
            this.ReloadAndRegisterToResult(state, ResultViewModelManager);
        }

        protected override void SaveStateToBundle(ICrossBundle bundle)
        {
            base.SaveStateToBundle(bundle);
            this.SaveRegisterToResult(bundle, ResultViewModelManager);
        }

        public override void ViewDestroy(bool viewFinishing = true)
        {
            base.ViewDestroy(viewFinishing);

            if (viewFinishing)
            {
                this.UnregisterToResult(ResultViewModelManager);
            }
        }

        public abstract bool ResultSet(ICrossResultSettingViewModel<TResult> viewModel, TResult result);
    }

    public abstract class MvxNavigationResultAwaitingViewModel<TParameter, TResult>
        : MvxNavigationResultAwaitingViewModel<TResult>, ICrossViewModel<TParameter>
    {
        protected MvxNavigationResultAwaitingViewModel(
                ILoggerFactory logFactory,
                ICrossNavigationService navigationService,
                ICrossResultViewModelManager resultViewModelManager)
            : base(logFactory, navigationService, resultViewModelManager)
        {
        }

        public abstract void Prepare(TParameter parameter);
    }

    public abstract class MvxNavigationResultSettingViewModel<TResult>
        : CrossNavigationViewModel, ICrossResultSettingViewModel<TResult>
    {
        protected ICrossResultViewModelManager ResultViewModelManager { get; }

        protected MvxNavigationResultSettingViewModel(
                ILoggerFactory logFactory,
                ICrossNavigationService navigationService,
                ICrossResultViewModelManager resultViewModelManager)
            : base(logFactory, navigationService)
        {
            ResultViewModelManager = resultViewModelManager;
        }

        public virtual void SetResult(TResult result)
        {
            this.SetResult<TResult>(result, ResultViewModelManager);
        }
    }

    public abstract class MvxNavigationResultSettingViewModel<TParameter, TResult>
        : MvxNavigationResultSettingViewModel<TResult>, ICrossViewModel<TParameter>
    {
        protected MvxNavigationResultSettingViewModel(
                ILoggerFactory logFactory,
                ICrossNavigationService navigationService,
                ICrossResultViewModelManager resultViewModelManager)
            : base(logFactory, navigationService, resultViewModelManager)
        {
        }

        public abstract void Prepare(TParameter parameter);
    }
}
