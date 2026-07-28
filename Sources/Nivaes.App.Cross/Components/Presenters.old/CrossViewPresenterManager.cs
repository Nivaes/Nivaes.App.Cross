using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross
{
    public abstract class CrossViewPresenterManager
        : ICrossViewPresenterManager
    {
        private readonly Dictionary<Type, Func<CrossPresentationHint, ValueTask<bool>>> _presentationHintHandlers = new();

        protected readonly ILogger Logger;

        public CrossViewPresenterManager(ILogger logger)
        {
            Logger = logger;
        }

        public virtual object? CreateOverridePresentationAttributeViewInstance(Type viewType)
        {
            return Activator.CreateInstance(viewType);
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

        public abstract BasePresentationAttribute CreatePresentationAttribute(IViewModelRequest request);

        public virtual BasePresentationAttribute? GetPresentationAttribute(IViewModelRequest request)
        {
            //var viewType = Singleton<ViewModelViewsKeyContainerManager>.Instance.GetValue(request.ViewModelType);

            var attribute = request.ViewType
                .GetCustomAttributes(typeof(BasePresentationAttribute), true)
                .FirstOrDefault();

            if (attribute is BasePresentationAttribute basePresentationAttribute)
            {
            //    if (basePresentationAttribute.ViewType == null)
            //        basePresentationAttribute.ViewType = viewType;

            //    if (basePresentationAttribute.ViewModelType == null)
            //        basePresentationAttribute.ViewModelType = request.ViewModelType;

                return basePresentationAttribute;
            }

            return CreatePresentationAttribute(request);
        }

        public virtual async ValueTask<bool> ChangePresentation(CrossPresentationHint hint)
        {
            if (await HandlePresentationChange(hint).ConfigureAwait(true))
                return true;

            if (hint is CrossClosePresentationHint presentationHint)
            {
                return await Close(presentationHint.ViewModelToClose).ConfigureAwait(true);
            }

            Logger.Log(LogLevel.Warning, "Hint ignored {Name}", hint.GetType().Name);
            return false;
        }

        public ValueTask<bool> Show(IViewModelRequest request)
        {
            var pressentationAction = GetPresentationAction(request, out var attribute);

            return pressentationAction.ShowAction(request, attribute);
        }

        public ValueTask<bool> Close(ICrossViewModel viewModel)
        {
            var request = new ViewModelRequest(viewModel);
            var pressentationAction = GetPresentationAction(request, out var attribute);

            return pressentationAction.CloseAction(request, attribute);
        }

        protected IPressenterAction GetPresentationAction(
            IViewModelRequest request, out BasePresentationAttribute attribute)
        {
            var presentationAttribute = GetPresentationAttribute(request);
            //presentationAttribute.ViewModelType = request.ViewModelType;
            var attributeType = presentationAttribute.GetType();

            attribute = presentationAttribute;

            return Singleton<PresentationAttributePresenterActionsKeyContainerManager>.Instance.GetValue(attributeType);
        }
    }
}
