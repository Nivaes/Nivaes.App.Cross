namespace Nivaes.App.Cross
{
    using System.Diagnostics.CodeAnalysis;

    public interface ICrossAttributeViewPresenterManager : ICrossViewPresenterManager
    {
        //ICrossViewModelTypeFinder? ViewModelTypeFinder { get; }
        //ICrossViewsContainer? ViewsContainer { get; }
        IDictionary<Type, CrossPresentationAttributeAction>? AttributeTypesToActionsDictionary { get; }
        void RegisterAttributeTypes();

        //TODO: Maybe move those to helper class
        BasePresentationAttribute GetPresentationAttribute(CrossViewModelRequest request);

        BasePresentationAttribute CreatePresentationAttribute(
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type? viewModelType,
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type? viewType);

        [Obsolete("No usar Override", true)]
        BasePresentationAttribute? GetOverridePresentationAttribute(
            CrossViewModelRequest request,
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.Interfaces)] Type viewType);
    }
}
