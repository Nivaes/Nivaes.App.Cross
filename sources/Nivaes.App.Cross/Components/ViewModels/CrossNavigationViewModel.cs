using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross
{
    public abstract class CrossNavigationViewModel
        : CrossViewModel
    {
        private static readonly ActivitySource Source = new("SampleCrossClient");

        protected readonly ICrossNavigationService NavigationService;

        protected CrossNavigationViewModel(ICrossNavigationService navigationService, ILogger logger)
            :base(logger)
        {
            NavigationService = navigationService;

            Logger.LogTrace($"Started {this.GetType().Name}");

            using var activity = Source.StartActivity("SampleCrossClient");

            activity?.SetTag("Prueba", this.GetType().Name);

            //await Task.Delay(1000);
            //Thread.Sleep(100);

            activity?.Stop();

            //Thread.Sleep(15000);
        }
    }

    public abstract class MvxNavigationViewModel<TParameter>
        : CrossNavigationViewModel, ICrossViewModel<TParameter>
    {
        protected MvxNavigationViewModel(ICrossNavigationService navigationService, ILogger logger)
            : base(navigationService, logger)
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
                ICrossNavigationService navigationService,
                ICrossResultViewModelManager resultViewModelManager,
                ILogger logger)
            : base(navigationService, logger)
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
            : base(navigationService, resultViewModelManager, logger)
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
            : base(navigationService, logger)
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
