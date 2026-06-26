using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross
{
    public abstract class CrossResultSettingViewModel<TResult>
        : CrossViewModel, ICrossResultSettingViewModel<TResult>
    {
        protected ICrossResultViewModelManager ResultViewModelManager { get; }

        protected CrossResultSettingViewModel(ICrossResultViewModelManager resultViewModelManager, ILogger logger)
            :base(logger)
        {
            ResultViewModelManager = resultViewModelManager;
        }

        public virtual void SetResult(TResult result)
        {
            this.SetResult<TResult>(result, ResultViewModelManager);
        }
    }

    public abstract class CrossResultSettingViewModel<TParameter, TResult> : CrossResultSettingViewModel<TResult>, ICrossViewModel<TParameter>
    {
        protected CrossResultSettingViewModel(ICrossResultViewModelManager resultViewModelManager, ILogger logger)
            : base(resultViewModelManager, logger)
        {
        }

        public abstract void Prepare(TParameter parameter);
    }
}