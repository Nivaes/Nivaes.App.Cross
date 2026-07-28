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

        protected abstract ValueTask<bool> ShowAction(IViewModelRequest request, TPresentationAttribute attribute);

        protected abstract ValueTask<bool> CloseAction(IViewModelRequest request, TPresentationAttribute attribute);

        [DebuggerHidden]
        public ValueTask<bool> ShowAction(IViewModelRequest request, IPresentationAttribute attribute)
        {
            return ShowAction(request, (TPresentationAttribute)attribute);
        }

        [DebuggerHidden]
        public ValueTask<bool> CloseAction(IViewModelRequest request, IPresentationAttribute attribute)
        {
            return CloseAction(request, (TPresentationAttribute)attribute);
        }

        private IPressenterAction GetPresentationAttributeAction(
            IViewModelRequest? request, out BasePresentationAttribute attribute)
        {
            var presentationAttribute = GetPresentationAttribute(request);
            var attributeType = presentationAttribute.GetType();

            attribute = presentationAttribute;

            return Singleton<PresentationAttributePresenterActionsKeyContainerManager>.Instance.GetValue(attributeType);
        }

        private BasePresentationAttribute GetPresentationAttribute(IViewModelRequest request)
        {
            var attribute = request.ViewType
                .GetCustomAttributes(typeof(BasePresentationAttribute), true)
                .FirstOrDefault();

            if (attribute is BasePresentationAttribute basePresentationAttribute)
            {
                return basePresentationAttribute;
            }

            return CreatePresentationAttribute(request);
        }

        protected abstract BasePresentationAttribute CreatePresentationAttribute(IViewModelRequest request);

        protected ValueTask<bool> Show(IViewModelRequest request)
        {
            var pressentationAction = GetPresentationAttributeAction(request, out var attribute);

            return pressentationAction.ShowAction(request, attribute);
        }

        protected ValueTask<bool> Close(ICrossViewModel viewModel)
        {
            var request = new ViewModelRequest(viewModel);
            var pressentationAction = GetPresentationAttributeAction(new ViewModelRequest(viewModel), out var attribute);

            return pressentationAction.CloseAction(request, attribute);
        }
    }
}
