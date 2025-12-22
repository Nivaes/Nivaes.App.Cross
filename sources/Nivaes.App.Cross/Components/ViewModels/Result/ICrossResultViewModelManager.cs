namespace Nivaes.App.Cross
{
    public interface ICrossResultViewModelManager
    {
        void RegisterToResult<TResult>(ICrossResultAwaitingViewModel<TResult> viewModel);

        bool UnregisterToResult<TResult>(ICrossResultAwaitingViewModel<TResult> viewModel);

        bool IsRegistered<TResult>(ICrossResultAwaitingViewModel<TResult> viewModel);

        void SetResult<TResult>(ICrossResultSettingViewModel<TResult> viewModel, TResult result);
    }
}