using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nivaes.App.Cross.Observability;

namespace Nivaes.App.Cross
{
    public abstract class CrossNavigationViewModel
        : CrossViewModel
    {
        private static Lazy<CrossNavigationService> _navigationService = 
            new Lazy<CrossNavigationService>(()=> IPlatformApplication.Current!.ServiceProvider.GetRequiredService<CrossNavigationService>());

        protected CrossNavigationService NavigationService => _navigationService.Value;

        protected Activity? Trace;

        protected CrossNavigationViewModel(ILogger logger)
            : base(logger)
        {
            Logger.LogTrace($"Started {this.GetType().FullName}");

            // Send Trace.
            Trace = Telemetry.ActivitySource.StartActivity("Load View");

            Trace?.SetTag(this.GetType().FullName!, "Load");

            try
            {
                Trace?.SetTag("success", true);
            }
            catch (Exception ex)
            {
                Trace?.AddException(ex);
                throw;
            }

            // Send Meter
            Telemetry.ButtonClicks.Add(
                1,
                new("screen", "Home"),
                new("platform", "Plataforma" /*DeviceInfo.Platform.ToString()*/));

            var sw = Stopwatch.StartNew();

            Thread.Sleep(150);

            sw.Stop();


            Telemetry.OperationDuration.Record(sw.Elapsed.TotalMilliseconds);
        }

        public override void ViewDestroy(bool viewFinishing = true)
        {
            base.ViewDestroy(viewFinishing);

            Trace?.SetTag("finalice", true);

            Trace?.Dispose();
            Trace = null;
        }
    }

    public abstract class CrossNavigationViewModel<TParameter>
        : CrossNavigationViewModel, ICrossViewModel<TParameter>
    {
        protected CrossNavigationViewModel(ILogger logger)
            : base(logger)
        {
        }

        public abstract void Prepare(TParameter parameter);
    }

    public abstract class CrossNavigationResultAwaitingViewModel<TResult>
        : CrossNavigationViewModel, ICrossResultAwaitingViewModel<TResult>
    {
        protected ICrossResultViewModelManager ResultViewModelManager { get; }

        protected CrossNavigationResultAwaitingViewModel(
                ICrossResultViewModelManager resultViewModelManager,
                ILogger logger)
            : base(logger)
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

    public abstract class CrossNavigationResultAwaitingViewModel<TParameter, TResult>
        : CrossNavigationResultAwaitingViewModel<TResult>, ICrossViewModel<TParameter>
    {
        protected CrossNavigationResultAwaitingViewModel(
                ICrossResultViewModelManager resultViewModelManager,
                ILogger logger)
            : base(resultViewModelManager, logger)
        {
        }

        public abstract void Prepare(TParameter parameter);
    }

    public abstract class CrossNavigationViewModelResult<TResult>
        : CrossNavigationViewModel, ICrossViewModelResult<TResult>
    {
        protected CrossNavigationViewModelResult(ILogger logger)
            : base(logger)
        {
        }

        public TaskCompletionSource<object>? CloseCompletionSource { get; set; }

        public override void ViewDestroy(bool viewFinishing = true)
        {
            if (viewFinishing && CloseCompletionSource != null && !CloseCompletionSource.Task.IsCompleted && !CloseCompletionSource.Task.IsFaulted)
                CloseCompletionSource?.TrySetCanceled();

            base.ViewDestroy(viewFinishing);
        }
    }

    public abstract class CrossNavigationViewModel<TParameter, TResult> :
        CrossNavigationViewModelResult<TResult>, ICrossViewModel<TParameter, TResult>
    {
        protected CrossNavigationViewModel(ILogger logger) :
            base(logger)
        {
        }

        public abstract void Prepare(TParameter parameter);
    }

    public abstract class CrossNavigationResultSettingViewModel<TResult>
        : CrossNavigationViewModel, ICrossResultSettingViewModel<TResult>
    {
        protected ICrossResultViewModelManager ResultViewModelManager { get; }

        protected CrossNavigationResultSettingViewModel(               
                ICrossResultViewModelManager resultViewModelManager,
                ILogger logger)
            : base(logger)
        {
            ResultViewModelManager = resultViewModelManager;
        }

        public virtual void SetResult(TResult result)
        {
            this.SetResult<TResult>(result, ResultViewModelManager);
        }
    }

    public abstract class CrossNavigationResultSettingViewModel<TParameter, TResult>
        : CrossNavigationResultSettingViewModel<TResult>, ICrossViewModel<TParameter>
    {
        protected CrossNavigationResultSettingViewModel(
                ICrossResultViewModelManager resultViewModelManager,
                ILogger logger
                )
            : base(resultViewModelManager, logger)
        {
        }

        public abstract void Prepare(TParameter parameter);
    }
}
