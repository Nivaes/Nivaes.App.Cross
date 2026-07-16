using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross
{
    public abstract class PressenterAction<TPresentationAttribute>
        : IPressenterAction
        where TPresentationAttribute : IPresentationAttribute
    {
        protected readonly ICrossViewsContainer ViewsContainer;
        protected readonly ILogger Logger;

        protected PressenterAction(ICrossViewsContainer viewsContainer, ILogger logger)
        {
            ViewsContainer = viewsContainer;
            Logger = logger;
        }

        protected abstract ValueTask<bool> ShowAction(Type viewType, TPresentationAttribute attribute, CrossViewModelRequest request);

        protected abstract ValueTask<bool> CloseAction(ICrossViewModel viewModel, TPresentationAttribute attribute);

        [DebuggerHidden]
        public ValueTask<bool> ShowAction(Type view, IPresentationAttribute attribute, CrossViewModelRequest request)
        {
            return ShowAction(view, (TPresentationAttribute)attribute, request);
        }

        [DebuggerHidden]
        public ValueTask<bool> CloseAction(ICrossViewModel viewModel, IPresentationAttribute attribute)
        {
            return CloseAction(viewModel, (TPresentationAttribute)attribute);
        }

        private IPressenterAction GetPresentationAttributeAction(
            CrossViewModelRequest? request, out BasePresentationAttribute attribute)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));

            var presentationAttribute = GetPresentationAttribute(request);
            presentationAttribute.ViewModelType = request.ViewModelType;
            var attributeType = presentationAttribute.GetType();

            attribute = presentationAttribute;

            return Singleton<PresentationAttributePresenterActionsKeyContainerManager>.Instance.GetValue(attributeType);
        }

        private BasePresentationAttribute GetPresentationAttribute(CrossViewModelRequest request)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));
            ArgumentNullException.ThrowIfNull(request.ViewModelType, nameof(request.ViewModelType));

            var viewType = ViewsContainer.GetViewType(request.ViewModelType);
            if (viewType == null)
                throw new InvalidOperationException($"Could not get View Type for ViewModel Type {request.ViewModelType}");

            //var overrideAttribute = GetOverridePresentationAttribute(request, viewType);
            //if (overrideAttribute != null)
            //    return overrideAttribute;

            var attribute = viewType
                .GetCustomAttributes(typeof(BasePresentationAttribute), true)
                .FirstOrDefault();

            if (attribute is BasePresentationAttribute basePresentationAttribute)
            {
                if (basePresentationAttribute.ViewType == null)
                    basePresentationAttribute.ViewType = viewType;

                if (basePresentationAttribute.ViewModelType == null)
                    basePresentationAttribute.ViewModelType = request.ViewModelType;

                return basePresentationAttribute;
            }

            return CreatePresentationAttribute(request.ViewModelType, viewType);
        }

        protected abstract BasePresentationAttribute CreatePresentationAttribute(Type? viewModelType, Type? viewType);

        protected ValueTask<bool> Show(CrossViewModelRequest request)
        {
            var pressentationAction = GetPresentationAttributeAction(request, out var attribute);

            return pressentationAction.ShowAction(attribute.ViewType!, attribute, request);
        }

        protected ValueTask<bool> Close(ICrossViewModel viewModel)
        {
            var pressentationAction = GetPresentationAttributeAction(new CrossViewModelInstanceRequest(viewModel), out var attribute);

            return pressentationAction.CloseAction(viewModel, attribute);
        }
    }
}
