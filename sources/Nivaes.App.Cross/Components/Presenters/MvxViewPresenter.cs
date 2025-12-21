namespace MvvmCross.Presenters
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross;

    public abstract class MvxViewPresenter 
        : IMvxViewPresenter
    {
        private readonly Dictionary<Type, Func<MvxPresentationHint, Task<bool>>> _presentationHintHandlers =
            new Dictionary<Type, Func<MvxPresentationHint, Task<bool>>>();

        public void AddPresentationHintHandler<THint>(Func<THint, Task<bool>> action)
            where THint : MvxPresentationHint
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            _presentationHintHandlers[typeof(THint)] = hint => action((THint)hint);
        }

        protected Task<bool> HandlePresentationChange(MvxPresentationHint hint)
        {
            if (hint == null)
                throw new ArgumentNullException(nameof(hint));

            if (_presentationHintHandlers.TryGetValue(
                hint.GetType(),
                out Func<MvxPresentationHint, Task<bool>> handler))
            {
                return handler(hint);
            }

            return Task.FromResult(false);
        }

        public abstract Task<bool> Show(CrossViewModelRequest request);

        public abstract Task<bool> ChangePresentation(MvxPresentationHint hint);

        public abstract Task<bool> Close(ICrossViewModel viewModel);
    }
}
