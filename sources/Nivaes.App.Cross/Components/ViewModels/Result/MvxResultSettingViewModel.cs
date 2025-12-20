namespace MvvmCross.ViewModels.Result
{
    using Nivaes.App.Cross;

    public abstract class MvxResultSettingViewModel<TResult>
        : CrossViewModel, IMvxResultSettingViewModel<TResult>
    {
        protected IMvxResultViewModelManager ResultViewModelManager { get; }

        protected MvxResultSettingViewModel(IMvxResultViewModelManager resultViewModelManager)
        {
            ResultViewModelManager = resultViewModelManager;
        }

        public virtual void SetResult(TResult result)
        {
            this.SetResult<TResult>(result, ResultViewModelManager);
        }
    }

    public abstract class MvxResultSettingViewModel<TParameter, TResult> : MvxResultSettingViewModel<TResult>, IMvxViewModel<TParameter>
    {
        protected MvxResultSettingViewModel(IMvxResultViewModelManager resultViewModelManager)
            : base(resultViewModelManager)
        {
        }

        public abstract void Prepare(TParameter parameter);
    }
}