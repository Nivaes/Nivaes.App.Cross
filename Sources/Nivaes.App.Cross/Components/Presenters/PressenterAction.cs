using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross
{
    public abstract class PressenterAction<TPresentationAttribute>
        : IPressenterAction
        where TPresentationAttribute : IPresentationAttribute
    {
        public ILogger Logger { get; }

        protected PressenterAction(ILogger logger)
        {
            Logger = logger;
        }

        protected abstract ValueTask<bool> ShowAction(Type viewType, TPresentationAttribute attribute, ViewModelRequest request);

        protected abstract ValueTask<bool> CloseAction(ICrossViewModel viewModel, TPresentationAttribute attribute);

        [DebuggerHidden]
        public ValueTask<bool> ShowAction(Type view, IPresentationAttribute attribute, ViewModelRequest request)
        {
            return ShowAction(view, (TPresentationAttribute)attribute, request);
        }

        [DebuggerHidden]
        public ValueTask<bool> CloseAction(ICrossViewModel viewModel, IPresentationAttribute attribute)
        {
            return CloseAction(viewModel, (TPresentationAttribute)attribute);
        }

        private IPressenterAction GetPresentationAttributeAction(
            ViewModelRequest? request, out BasePresentationAttribute attribute)
        {
            var presentationAttribute = GetPresentationAttribute(request);
            presentationAttribute.ViewModelType = request.ViewModelType;
            var attributeType = presentationAttribute.GetType();

            attribute = presentationAttribute;

            return Singleton<PresentationAttributePresenterActionsKeyContainerManager>.Instance.GetValue(attributeType);
        }

        private BasePresentationAttribute GetPresentationAttribute(ViewModelRequest request)
        {
            var viewType = Singleton<ViewModelViewsKeyContainerManager>.Instance
                    .GetValue(request.ViewModelType);

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

        protected abstract BasePresentationAttribute CreatePresentationAttribute(Type viewModelType, Type viewType);

        protected ValueTask<bool> Show(ViewModelRequest request)
        {
            var pressentationAction = GetPresentationAttributeAction(request, out var attribute);

            return pressentationAction.ShowAction(attribute.ViewType!, attribute, request);
        }

        protected ValueTask<bool> Close(ICrossViewModel viewModel)
        {
            var pressentationAction = GetPresentationAttributeAction(new ViewModelRequest(viewModel), out var attribute);

            return pressentationAction.CloseAction(viewModel, attribute);
        }
    }
}
