using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross
{
    public abstract class PressenterAction<TPressenterAttribute>
        : IPressenterAction
        where TPressenterAttribute : IPresentationAttribute
    {
        protected readonly ICrossViewsContainer ViewsContainer;
        protected readonly ILogger Logger;

        protected PressenterAction(ICrossViewsContainer viewsContainer, ILogger logger)
        {
            ViewsContainer = viewsContainer;
            Logger = logger;
        }

        protected abstract ValueTask<bool> ShowAction(Type viewType, TPressenterAttribute attribute, CrossViewModelRequest request);

        protected abstract ValueTask<bool> CloseAction(ICrossViewModel viewModel, TPressenterAttribute attribute);

        public ValueTask<bool> ShowActon(Type view, IPresentationAttribute attribute, CrossViewModelRequest request)
        {
            return ShowActon(view, attribute, request);
        }

        public ValueTask<bool> CloseActon(ICrossViewModel viewModel, IPresentationAttribute attribute)
        {
            return CloseActon(viewModel, attribute);
        }

        [Obsolete]
        private CrossPresentationAttributeAction GetPresentationAttributeAction(
            CrossViewModelRequest? request, out BasePresentationAttribute attribute)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));

            var presentationAttribute = GetPresentationAttribute(request);
            presentationAttribute.ViewModelType = request.ViewModelType;
            var attributeType = presentationAttribute.GetType();

            attribute = presentationAttribute;

            throw new NotImplementedException();

            //if (AttributeTypesToActionsDictionary.TryGetValue(attributeType, out var attributeAction))
            //{
            //    if (attributeAction.ShowAction == null)
            //    {
            //        throw new InvalidOperationException(
            //            $"attributeAction.ShowAction is null for attribute: {attributeType.Name}");
            //    }

            //    if (attributeAction.CloseAction == null)
            //    {
            //        throw new InvalidOperationException(
            //            $"attributeAction.CloseAction is null for attribute: {attributeType.Name}");
            //    }

            //    return attributeAction;
            //}

            throw new KeyNotFoundException($"The type {attributeType.Name} is not configured in the presenter dictionary");
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

        protected Task<bool> Close(ICrossViewModel viewModel)
        {
            return GetPresentationAttributeAction(
                    new CrossViewModelInstanceRequest(viewModel), out var attribute)
                .CloseAction?
                .Invoke(viewModel, attribute) ?? Task.FromResult(false);
        }

        protected Task<bool> Show(CrossViewModelRequest request)
        {
            var attributeAction = GetPresentationAttributeAction(request, out var attribute);

            if (attributeAction.ShowAction != null && attribute.ViewType != null)
                return attributeAction.ShowAction.Invoke(attribute.ViewType, attribute, request);

            return Task.FromResult(false);
        }
    }
}
