using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross
{
    public abstract class CrossViewPresenterManager
        : ICrossViewPresenterManager
    {
        private readonly Dictionary<Type, Func<CrossPresentationHint, ValueTask<bool>>> _presentationHintHandlers =
           new Dictionary<Type, Func<CrossPresentationHint, ValueTask<bool>>>();

        protected readonly ILogger Logger;

        public CrossViewPresenterManager(ILogger logger)
        {
            Logger = logger;
        }

        public void AddPresentationHintHandler<THint>(Func<THint, ValueTask<bool>> action)
            where THint : CrossPresentationHint
        {
            _presentationHintHandlers[typeof(THint)] = hint => action((THint)hint);
        }

        protected ValueTask<bool> HandlePresentationChange(CrossPresentationHint hint)
        {
            if (_presentationHintHandlers.TryGetValue(hint.GetType(), out var handler))
            {
                return handler(hint);
            }

            return ValueTask.FromResult(false);
        }

        public abstract ValueTask<bool> Show(CrossViewModelRequest request);

        public abstract ValueTask<bool> ChangePresentation(CrossPresentationHint hint);

        public abstract ValueTask<bool> Close(ICrossViewModel viewModel);
    }
}
