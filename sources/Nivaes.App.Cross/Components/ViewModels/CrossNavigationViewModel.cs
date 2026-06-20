namespace Nivaes.App.Cross
{
    using System.Diagnostics;
    using Microsoft.Extensions.Logging;
    using OpenTelemetry.Resources;
    using OpenTelemetry.Trace;

    public abstract class CrossNavigationViewModel
        : CrossViewModel
    {
        protected ILogger Logger { [DebuggerHidden] get; }

        private static readonly ActivitySource Source = new("SampleCrossClient");

        protected CrossNavigationViewModel(ILogger logger, ICrossNavigationService navigationService)
        {
            Logger = logger;
            NavigationService = navigationService;

            Logger.LogTrace($"Se inicio {this.GetType().Name}");

            using var activity = Source.StartActivity("SampleCrossClient");

            activity?.SetTag("Prueba", this.GetType().Name);

            //await Task.Delay(1000);
            //Thread.Sleep(100);

            activity?.Stop();

            //Thread.Sleep(15000);
        }

        protected virtual ICrossNavigationService NavigationService { [DebuggerHidden]get; }
    }

    public abstract class MvxNavigationViewModel<TParameter>
        : CrossNavigationViewModel, ICrossViewModel<TParameter>
    {
        protected MvxNavigationViewModel(ILogger logger, ICrossNavigationService navigationService)
            : base(logger, navigationService)
        {
            logger.LogTrace($"Se inicio {this.GetType().Name}");
        }

        public abstract void Prepare(TParameter parameter);
    }

    public abstract class MvxNavigationResultAwaitingViewModel<TResult>
        : CrossNavigationViewModel, ICrossResultAwaitingViewModel<TResult>
    {
        protected ICrossResultViewModelManager ResultViewModelManager { get; }

        protected MvxNavigationResultAwaitingViewModel(
                ILogger logger,
                ICrossNavigationService navigationService,
                ICrossResultViewModelManager resultViewModelManager)
            : base(logger, navigationService)
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
                ILogger logger,
                ICrossNavigationService navigationService,
                ICrossResultViewModelManager resultViewModelManager)
            : base(logger, navigationService, resultViewModelManager)
        {
        }

        public abstract void Prepare(TParameter parameter);
    }

    public abstract class MvxNavigationResultSettingViewModel<TResult>
        : CrossNavigationViewModel, ICrossResultSettingViewModel<TResult>
    {
        protected ICrossResultViewModelManager ResultViewModelManager { get; }

        protected MvxNavigationResultSettingViewModel(
                ILogger logger,
                ICrossNavigationService navigationService,
                ICrossResultViewModelManager resultViewModelManager)
            : base(logger, navigationService)
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
                ILogger logger,
                ICrossNavigationService navigationService,
                ICrossResultViewModelManager resultViewModelManager)
            : base(logger, navigationService, resultViewModelManager)
        {
        }

        public abstract void Prepare(TParameter parameter);
    }
}
