namespace MvvmCross.ViewModels.Result
{
    using Nivaes.App.Cross;

    public abstract class CrossResultSettingViewModel<TResult>
        : CrossViewModel, ICrossResultSettingViewModel<TResult>
    {
        protected ICrossResultViewModelManager ResultViewModelManager { get; }

        protected CrossResultSettingViewModel(ICrossResultViewModelManager resultViewModelManager)
        {
            ResultViewModelManager = resultViewModelManager;
        }

        public virtual void SetResult(TResult result)
        {
            this.SetResult<TResult>(result, ResultViewModelManager);
        }
    }

    public abstract class MvxResultSettingViewModel<TParameter, TResult> : CrossResultSettingViewModel<TResult>, ICrossViewModel<TParameter>
    {
        protected MvxResultSettingViewModel(ICrossResultViewModelManager resultViewModelManager)
            : base(resultViewModelManager)
        {
        }

        public abstract void Prepare(TParameter parameter);
    }
}