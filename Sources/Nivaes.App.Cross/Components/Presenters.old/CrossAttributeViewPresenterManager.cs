using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross;

public abstract class CrossAttributeViewPresenterManager
    : CrossViewPresenterManager, ICrossAttributeViewPresenterManager
{
    protected readonly ICrossViewsContainer ViewsContainer;

    protected CrossAttributeViewPresenterManager(ICrossViewsContainer crossViewsContainer, ILogger logger)
        : base(logger)
    {
        ViewsContainer = crossViewsContainer;
    }

    //[Obsolete("", true)]
    //private IDictionary<Type, CrossPresentationAttributeAction>? _attributeTypesActionsDictionary;

    //[Obsolete("Busca interfaces de la vista.", true)]
    //public virtual IDictionary<Type, CrossPresentationAttributeAction> AttributeTypesToActionsDictionary
    //{
    //    get
    //    {
    //        throw new NotImplementedException();
    //        if (_attributeTypesActionsDictionary == null)
    //        {
    //            _attributeTypesActionsDictionary = new Dictionary<Type, CrossPresentationAttributeAction>();
    //            RegisterAttributeTypes();
    //        }
    //        return _attributeTypesActionsDictionary;
    //    }
    //}

    //[Obsolete("", true)]
    //public abstract void RegisterAttributeTypes();

    public abstract BasePresentationAttribute CreatePresentationAttribute(
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type? viewModelType,
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type? viewType);

    public virtual object? CreateOverridePresentationAttributeViewInstance(
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type viewType)
    {
        return Activator.CreateInstance(viewType);
    }

    [Obsolete("Busca interfaces de la vista.", true)]
    public virtual BasePresentationAttribute? GetOverridePresentationAttribute(
        CrossViewModelRequest request,
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.Interfaces)] Type viewType)
    {
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

    public virtual BasePresentationAttribute GetPresentationAttribute(CrossViewModelRequest request)
    {
        var viewType = ViewsContainer.GetViewType(request.ViewModelType);
        if (viewType == null)
            throw new InvalidOperationException($"Could not get View Type for ViewModel Type {request.ViewModelType}");

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

    //[Obsolete("Migrate to PressenterAction", true)]
    //protected virtual CrossPresentationAttributeAction GetPresentationAttributeAction(
    //    CrossViewModelRequest? request, out BasePresentationAttribute attribute)
    //{
    //    var presentationAttribute = GetPresentationAttribute(request);
    //    presentationAttribute.ViewModelType = request.ViewModelType;
    //    var attributeType = presentationAttribute.GetType();

    //    attribute = presentationAttribute;

    //    if (AttributeTypesToActionsDictionary.TryGetValue(attributeType, out var attributeAction))
    //    {
    //        if (attributeAction.ShowAction == null)
    //        {
    //            throw new InvalidOperationException(
    //                $"attributeAction.ShowAction is null for attribute: {attributeType.Name}");
    //        }

    //        if (attributeAction.CloseAction == null)
    //        {
    //            throw new InvalidOperationException(
    //                $"attributeAction.CloseAction is null for attribute: {attributeType.Name}");
    //        }

    //        return attributeAction;
    //    }

    //    throw new KeyNotFoundException($"The type {attributeType.Name} is not configured in the presenter dictionary");
    //}

    public override async ValueTask<bool> ChangePresentation(CrossPresentationHint hint)
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

    public override ValueTask<bool> Show(CrossViewModelRequest request)
    {
        var pressentationAction = GetPresentationAction(request, out var attribute);

        return pressentationAction.ShowAction(attribute.ViewType!, attribute, request);
    }

    public override ValueTask<bool> Close(ICrossViewModel viewModel)
    {
        var pressentationAction = GetPresentationAction(new CrossViewModelInstanceRequest(viewModel), out var attribute);

        return pressentationAction.CloseAction(viewModel, attribute);
    }

    protected virtual IPressenterAction GetPresentationAction(
        CrossViewModelRequest? request, out BasePresentationAttribute attribute)
    {
        var presentationAttribute = GetPresentationAttribute(request);
        presentationAttribute.ViewModelType = request.ViewModelType;
        var attributeType = presentationAttribute.GetType();

        attribute = presentationAttribute;

        return Singleton<PresentationAttributePresenterActionsKeyContainerManager>.Instance.GetValue(attributeType);
    }

    //public override async ValueTask<bool> ChangePresentation(CrossPresentationHint hint)
    //{
    //    if (await HandlePresentationChange(hint).ConfigureAwait(true))
    //        return true;

    //    if (hint is CrossClosePresentationHint presentationHint)
    //    {
    //        return await Close(presentationHint.ViewModelToClose).ConfigureAwait(true);
    //    }

    //    Logger.Log(LogLevel.Warning, "Hint ignored {Name}", hint.GetType().Name);
    //    return false;
    //}

    ////[Obsolete("Migrate to PressenterAction", true)]
    //public override ValueTask<bool> Close(ICrossViewModel viewModel)
    //{
    //    var pressentationAction = GetPresentationAction(new CrossViewModelInstanceRequest(viewModel), out var attribute);

    //   return pressentationAction.CloseActon(viewModel, attribute);
    //}

    ////[Obsolete("Migrate to PressenterAction", true)]
    //public override ValueTask<bool> Show(CrossViewModelRequest request)
    //{
    //    var pressentationAction = GetPresentationAction(request, out var attribute);

    //    return pressentationAction.ShowAction(attribute.ViewType!, attribute, request);
    //}
}