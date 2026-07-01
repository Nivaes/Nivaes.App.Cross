namespace Nivaes.App.Cross
{

    public static class CrossResultViewModelExtensions
    {
        public const string BundleRegisterKey = "__mvxResultVMRegisterKey";

        public static void ReloadAndRegisterToResult<TResult>(
            this ICrossResultAwaitingViewModel<TResult> viewModel,
            ICrossBundle savedStateBundle,
            ICrossResultViewModelManager resultViewModelManager)
        {
            if (savedStateBundle?.Data.TryGetValue(BundleRegisterKey, out string? restoreRegisterStr) == true &&
                bool.TryParse(restoreRegisterStr, out bool restoreRegister) && restoreRegister)
            {
                resultViewModelManager.RegisterToResult(viewModel);
            }
        }

        public static void SaveRegisterToResult<TResult>(
            this ICrossResultAwaitingViewModel<TResult> viewModel,
            ICrossBundle savedStateBundle,
            ICrossResultViewModelManager resultViewModelManager)
        {
            if (resultViewModelManager.IsRegistered(viewModel) &&
                savedStateBundle?.Data is { } data)
            {
                data[BundleRegisterKey] = true.ToString();
            }
        }

        public static void RegisterToResult<TResult>(
            this ICrossResultAwaitingViewModel<TResult> viewModel,
            ICrossResultViewModelManager resultViewModelManager)
        {
            resultViewModelManager.RegisterToResult(viewModel);
        }

        public static void UnregisterToResult<TResult>(
            this ICrossResultAwaitingViewModel<TResult> viewModel,
            ICrossResultViewModelManager resultViewModelManager)
        {
            resultViewModelManager.UnregisterToResult(viewModel);
        }

        public static void SetResult<TResult>(
            this ICrossResultSettingViewModel<TResult> viewModel,
            TResult result,
            ICrossResultViewModelManager resultViewModelManager)
        {
            resultViewModelManager.SetResult(viewModel, result);
        }
    }
}