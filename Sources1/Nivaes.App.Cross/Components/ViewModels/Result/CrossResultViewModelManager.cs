namespace Nivaes.App.Cross
{
    public class CrossResultViewModelManager : ICrossResultViewModelManager
    {
        private readonly Dictionary<Type, HashSet<IMvxBaseResultAwaitingViewModel>> _registrations = new();

        public void RegisterToResult<TResult>(ICrossResultAwaitingViewModel<TResult> viewModel)
        {
            if (!_registrations.TryGetValue(typeof(TResult), out HashSet<IMvxBaseResultAwaitingViewModel>? resultRegistrations))
            {
                resultRegistrations = new();
                _registrations[typeof(TResult)] = resultRegistrations;
            }

            resultRegistrations.Add(viewModel);
        }

        public bool UnregisterToResult<TResult>(ICrossResultAwaitingViewModel<TResult> viewModel)
        {
            if (_registrations.TryGetValue(typeof(TResult), out HashSet<IMvxBaseResultAwaitingViewModel>? resultRegistrations) &&
                resultRegistrations.Remove(viewModel))
            {
                return true;
            }
            return false;
        }

        public bool IsRegistered<TResult>(ICrossResultAwaitingViewModel<TResult> viewModel)
        {
            if (_registrations.TryGetValue(typeof(TResult), out HashSet<IMvxBaseResultAwaitingViewModel>? resultRegistrations) &&
                resultRegistrations.Contains(viewModel))
            {
                return true;
            }
            return false;
        }

        public void SetResult<TResult>(ICrossResultSettingViewModel<TResult> viewModel, TResult result)
        {
            if (_registrations.TryGetValue(typeof(TResult), out HashSet<IMvxBaseResultAwaitingViewModel>? resultRegistrations))
            {
                foreach (var resultRegistration in resultRegistrations.Cast<ICrossResultAwaitingViewModel<TResult>>().ToArray())
                {
                    if (resultRegistration.ResultSet(viewModel, result))
                    {
                        resultRegistrations.Remove(resultRegistration);
                    }
                }
            }
        }
    }
}