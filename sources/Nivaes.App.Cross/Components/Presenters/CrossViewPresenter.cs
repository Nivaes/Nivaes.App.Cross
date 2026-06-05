namespace Nivaes.App.Cross
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public abstract class CrossViewPresenter 
        : ICrossViewPresenter
    {
        private readonly Dictionary<Type, Func<CrossPresentationHint, Task<bool>>> _presentationHintHandlers =
            new Dictionary<Type, Func<CrossPresentationHint, Task<bool>>>();

        public void AddPresentationHintHandler<THint>(Func<THint, Task<bool>> action)
            where THint : CrossPresentationHint
        {
            ArgumentNullException.ThrowIfNull(action, nameof(action));

            _presentationHintHandlers[typeof(THint)] = hint => action((THint)hint);
        }

        protected Task<bool> HandlePresentationChange(CrossPresentationHint hint)
        {
            ArgumentNullException.ThrowIfNull(hint, nameof(hint));

            if (_presentationHintHandlers.TryGetValue(
                hint.GetType(),
                out Func<CrossPresentationHint, Task<bool>> handler))
            {
                return handler(hint);
            }

            return Task.FromResult(false);
        }

        public abstract Task<bool> Show(CrossViewModelRequest request);

        public abstract Task<bool> ChangePresentation(CrossPresentationHint hint);

        public abstract Task<bool> Close(ICrossViewModel viewModel);
    }
}
