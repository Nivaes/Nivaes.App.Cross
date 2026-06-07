using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;
using Nivaes.IoC;

namespace Nivaes.App.Cross;

public abstract class CrossAttributeViewPresenter
    : CrossViewPresenter, ICrossAttributeViewPresenter
{
    protected ICrossViewsContainer ViewsContainer { get; }

    protected CrossAttributeViewPresenter(/*IServiceProvider serviceProvider*/ICrossViewsContainer crossViewsContainer)
    {
        ViewsContainer = crossViewsContainer;
    }

    //private readonly Lazy<ICrossViewModelTypeFinder?> _viewModelTypeFinder =
    //    new(() => Mvx.IoCProvider?.Resolve<ICrossViewModelTypeFinder>());

    //private readonly Lazy<ICrossViewsContainer?> _viewsContainer =
    //    new(() => Mvx.IoCProvider?.Resolve<ICrossViewsContainer>());

    private IDictionary<Type, CrossPresentationAttributeAction>? _attributeTypesActionsDictionary;

    //public virtual ICrossViewModelTypeFinder? ViewModelTypeFinder => _viewModelTypeFinder.Value;

    //public virtual ICrossViewsContainer? ViewsContainer => _viewsContainer.Value;

    public virtual IDictionary<Type, CrossPresentationAttributeAction> AttributeTypesToActionsDictionary
    {
        get
        {
            if (_attributeTypesActionsDictionary == null)
            {
                _attributeTypesActionsDictionary = new Dictionary<Type, CrossPresentationAttributeAction>();
                RegisterAttributeTypes();
            }
            return _attributeTypesActionsDictionary;
        }
    }

    public abstract void RegisterAttributeTypes();

    public abstract CrossBasePresentationAttribute CreatePresentationAttribute(
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type? viewModelType,
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type? viewType);

    public virtual object? CreateOverridePresentationAttributeViewInstance(
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type viewType)
    {
        ArgumentNullException.ThrowIfNull(viewType, nameof(viewType));

        return Activator.CreateInstance(viewType);
    }

    public virtual CrossBasePresentationAttribute? GetOverridePresentationAttribute(
        CrossViewModelRequest request,
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.Interfaces)] Type viewType)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));
        ArgumentNullException.ThrowIfNull(viewType, nameof(viewType));

        var hasInterface = viewType.GetInterfaces().Contains(typeof(ICrossOverridePresentationAttribute));
        if (!hasInterface)
            return null;

        var viewInstance =
            CreateOverridePresentationAttributeViewInstance(viewType) as ICrossOverridePresentationAttribute;
        try
        {
            var presentationAttribute = viewInstance?.PresentationAttribute(request);
            if (presentationAttribute == null)
                return null;

            if (presentationAttribute.ViewType == null)
            {
                presentationAttribute.ViewType = viewType;
            }

            if (presentationAttribute.ViewModelType == null)
            {
                presentationAttribute.ViewModelType = request.ViewModelType;
            }

            return presentationAttribute;
        }
        finally
        {
            (viewInstance as IDisposable)?.Dispose();
        }
    }

    public virtual CrossBasePresentationAttribute GetPresentationAttribute(CrossViewModelRequest request)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));
        ArgumentNullException.ThrowIfNull(request.ViewModelType, nameof(request.ViewModelType));

        //if (ViewsContainer == null)
        //    throw new InvalidOperationException($"Cannot get view types from null {nameof(ViewsContainer)}");

        var viewType = ViewsContainer.GetViewType(request.ViewModelType);
        if (viewType == null)
            throw new InvalidOperationException($"Could not get View Type for ViewModel Type {request.ViewModelType}");

        var overrideAttribute = GetOverridePresentationAttribute(request, viewType);
        if (overrideAttribute != null)
            return overrideAttribute;

        var attribute = viewType
            .GetCustomAttributes(typeof(CrossBasePresentationAttribute), true)
            .FirstOrDefault();

        if (attribute is CrossBasePresentationAttribute basePresentationAttribute)
        {
            if (basePresentationAttribute.ViewType == null)
                basePresentationAttribute.ViewType = viewType;

            if (basePresentationAttribute.ViewModelType == null)
                basePresentationAttribute.ViewModelType = request.ViewModelType;

            return basePresentationAttribute;
        }

        return CreatePresentationAttribute(request.ViewModelType, viewType);
    }

    protected virtual CrossPresentationAttributeAction GetPresentationAttributeAction(
        CrossViewModelRequest? request, out CrossBasePresentationAttribute attribute)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        var presentationAttribute = GetPresentationAttribute(request);
        presentationAttribute.ViewModelType = request.ViewModelType;
        var attributeType = presentationAttribute.GetType();

        attribute = presentationAttribute;

        if (AttributeTypesToActionsDictionary.TryGetValue(attributeType, out var attributeAction))
        {
            if (attributeAction.ShowAction == null)
            {
                throw new InvalidOperationException(
                    $"attributeAction.ShowAction is null for attribute: {attributeType.Name}");
            }

            if (attributeAction.CloseAction == null)
            {
                throw new InvalidOperationException(
                    $"attributeAction.CloseAction is null for attribute: {attributeType.Name}");
            }

            return attributeAction;
        }

        throw new KeyNotFoundException($"The type {attributeType.Name} is not configured in the presenter dictionary");
    }

    public override async Task<bool> ChangePresentation(CrossPresentationHint hint)
    {
        if (await HandlePresentationChange(hint).ConfigureAwait(true))
            return true;

        if (hint is CrossClosePresentationHint presentationHint)
        {
            return await Close(presentationHint.ViewModelToClose).ConfigureAwait(true);
        }

        CrossLogHost.Default?.Log(LogLevel.Warning, "Hint ignored {Name}", hint.GetType().Name);
        return false;
    }

    public override Task<bool> Close(ICrossViewModel viewModel)
    {
        return GetPresentationAttributeAction(
                new CrossViewModelInstanceRequest(viewModel), out var attribute)
            .CloseAction?
            .Invoke(viewModel, attribute) ?? Task.FromResult(false);
    }

    public override Task<bool> Show(CrossViewModelRequest request)
    {
        var attributeAction = GetPresentationAttributeAction(request, out var attribute);

        if (attributeAction.ShowAction != null && attribute.ViewType != null)
            return attributeAction.ShowAction.Invoke(attribute.ViewType, attribute, request);

        return Task.FromResult(false);
    }
}