namespace MvvmCross.ViewModels
{
    using Microsoft.Extensions.Logging;
    using MvvmCross.ViewModels.Result;
    using Nivaes.App.Cross;

    public abstract class MvxNavigationViewModel
        : CrossViewModel
    {
        private ILogger? _log;

        protected MvxNavigationViewModel(ILoggerFactory logFactory, ICrossNavigationService navigationService)
        {
            LoggerFactory = logFactory;
            NavigationService = navigationService;
        }

        protected virtual ICrossNavigationService NavigationService { get; }

        protected virtual ILoggerFactory LoggerFactory { get; }

        protected virtual ILogger Log => _log ??= LoggerFactory.CreateLogger(GetType().Name);
    }

    public abstract class MvxNavigationViewModel<TParameter>
        : MvxNavigationViewModel, ICrossViewModel<TParameter>
    {
        protected MvxNavigationViewModel(ILoggerFactory logFactory, ICrossNavigationService navigationService)
            : base(logFactory, navigationService)
        {
        }

        public abstract void Prepare(TParameter parameter);
    }

    public abstract class MvxNavigationResultAwaitingViewModel<TResult>
        : MvxNavigationViewModel, IMvxResultAwaitingViewModel<TResult>
    {
        protected IMvxResultViewModelManager ResultViewModelManager { get; }

        protected MvxNavigationResultAwaitingViewModel(
                ILoggerFactory logFactory,
                ICrossNavigationService navigationService,
                IMvxResultViewModelManager resultViewModelManager)
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

        public abstract bool ResultSet(IMvxResultSettingViewModel<TResult> viewModel, TResult result);
    }

    public abstract class MvxNavigationResultAwaitingViewModel<TParameter, TResult>
        : MvxNavigationResultAwaitingViewModel<TResult>, ICrossViewModel<TParameter>
    {
        protected MvxNavigationResultAwaitingViewModel(
                ILoggerFactory logFactory,
                ICrossNavigationService navigationService,
                IMvxResultViewModelManager resultViewModelManager)
            : base(logFactory, navigationService, resultViewModelManager)
        {
        }

        public abstract void Prepare(TParameter parameter);
    }

    public abstract class MvxNavigationResultSettingViewModel<TResult>
        : MvxNavigationViewModel, IMvxResultSettingViewModel<TResult>
    {
        protected IMvxResultViewModelManager ResultViewModelManager { get; }

        protected MvxNavigationResultSettingViewModel(
                ILoggerFactory logFactory,
                ICrossNavigationService navigationService,
                IMvxResultViewModelManager resultViewModelManager)
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
                IMvxResultViewModelManager resultViewModelManager)
            : base(logFactory, navigationService, resultViewModelManager)
        {
        }

        public abstract void Prepare(TParameter parameter);
    }
}
