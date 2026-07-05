using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross
{
    public abstract class CrossResultAwaitingViewModel<TResult>
        : CrossViewModel, ICrossResultAwaitingViewModel<TResult>
    {
        protected ICrossResultViewModelManager ResultViewModelManager { get; }

        protected CrossResultAwaitingViewModel(ICrossResultViewModelManager resultViewModelManager, ILogger logger)
            : base(logger)
        {
            ResultViewModelManager = resultViewModelManager;
        }

        protected override void ReloadFromBundle(ICrossBundle state)
        {
            base.ReloadFromBundle(state);
            ReloadAndRegisterToResult(state);
        }

        protected override void SaveStateToBundle(ICrossBundle bundle)
        {
            base.SaveStateToBundle(bundle);
            SaveRegisterToResult(bundle);
        }

        public override void ViewDestroy(bool viewFinishing = true)
        {
            base.ViewDestroy(viewFinishing);

            if (viewFinishing)
            {
                UnregisterToResult();
            }
        }

        public virtual void ReloadAndRegisterToResult(ICrossBundle state)
        {
            this.ReloadAndRegisterToResult(state, ResultViewModelManager);
        }

        public virtual void SaveRegisterToResult(ICrossBundle state)
        {
            this.SaveRegisterToResult<TResult>(state, ResultViewModelManager);
        }

        public virtual void UnregisterToResult()
        {
            this.UnregisterToResult<TResult>(ResultViewModelManager);
        }

        public abstract bool ResultSet(ICrossResultSettingViewModel<TResult> viewModel, TResult result);
    }

    public abstract class MvxResultAwaitingViewModel<TParameter, TResult>
        : CrossResultAwaitingViewModel<TResult>, ICrossViewModel<TParameter>
    {
        protected MvxResultAwaitingViewModel(ICrossResultViewModelManager resultViewModelManager, ILogger logger)
            : base(resultViewModelManager, logger)
        {
        }

        public abstract void Prepare(TParameter parameter);
    }

    public abstract class MvxResultAwaitingViewModel<TParameter, TResult1, TResult2>
        : MvxMultiResultAwaitingViewModel<TResult1, TResult2>, ICrossViewModel<TParameter>
    {
        protected MvxResultAwaitingViewModel(ICrossResultViewModelManager resultViewModelManager, ILogger logger)
            : base(resultViewModelManager, logger)
        {
        }

        public abstract void Prepare(TParameter parameter);
    }

    public abstract class MvxResultAwaitingViewModel<TParameter, TResult1, TResult2, TResult3>
        : MvxMultiResultAwaitingViewModel<TResult1, TResult2, TResult3>, ICrossViewModel<TParameter>
    {
        protected MvxResultAwaitingViewModel(ICrossResultViewModelManager resultViewModelManager, ILogger logger)
            : base(resultViewModelManager, logger)
        {
        }

        public abstract void Prepare(TParameter parameter);
    }

    public abstract class MvxMultiResultAwaitingViewModel<TResult1, TResult2>
        : CrossResultAwaitingViewModel<TResult1>, ICrossResultAwaitingViewModel<TResult2>
    {
        protected MvxMultiResultAwaitingViewModel(ICrossResultViewModelManager resultViewModelManager, ILogger logger)
            : base(resultViewModelManager, logger)
        {
        }

        public override void ReloadAndRegisterToResult(ICrossBundle state)
        {
            base.ReloadAndRegisterToResult(state);
            this.ReloadAndRegisterToResult<TResult2>(state, ResultViewModelManager);
        }

        public override void SaveRegisterToResult(ICrossBundle state)
        {
            base.SaveRegisterToResult(state);
            this.SaveRegisterToResult<TResult2>(state, ResultViewModelManager);
        }

        public override void UnregisterToResult()
        {
            base.UnregisterToResult();
            this.UnregisterToResult<TResult2>(ResultViewModelManager);
        }

        public abstract bool ResultSet(ICrossResultSettingViewModel<TResult2> viewModel, TResult2 result);
    }

    public abstract class MvxMultiResultAwaitingViewModel<TResult1, TResult2, TResult3>
        : MvxMultiResultAwaitingViewModel<TResult1, TResult2>, ICrossResultAwaitingViewModel<TResult3>
    {
        protected MvxMultiResultAwaitingViewModel(ICrossResultViewModelManager resultViewModelManager, ILogger logger)
            : base(resultViewModelManager, logger)
        {
        }

        public override void ReloadAndRegisterToResult(ICrossBundle state)
        {
            base.ReloadAndRegisterToResult(state);
            this.ReloadAndRegisterToResult<TResult3>(state, ResultViewModelManager);
        }

        public override void SaveRegisterToResult(ICrossBundle state)
        {
            base.SaveRegisterToResult(state);
            this.SaveRegisterToResult<TResult3>(state, ResultViewModelManager);
        }

        public override void UnregisterToResult()
        {
            base.UnregisterToResult();
            this.UnregisterToResult<TResult3>(ResultViewModelManager);
        }

        public abstract bool ResultSet(ICrossResultSettingViewModel<TResult3> viewModel, TResult3 result);
    }
}