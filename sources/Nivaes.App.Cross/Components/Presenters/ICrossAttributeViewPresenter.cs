namespace Nivaes.App.Cross
{
    using System.Diagnostics.CodeAnalysis;

    public interface ICrossAttributeViewPresenter : ICrossViewPresenter
    {
        ICrossViewModelTypeFinder? ViewModelTypeFinder { get; }
        ICrossViewsContainer? ViewsContainer { get; }
        IDictionary<Type, CrossPresentationAttributeAction>? AttributeTypesToActionsDictionary { get; }
        void RegisterAttributeTypes();

        //TODO: Maybe move those to helper class
        CrossBasePresentationAttribute GetPresentationAttribute(CrossViewModelRequest request);

        CrossBasePresentationAttribute CreatePresentationAttribute(
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type? viewModelType,
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type? viewType);

        CrossBasePresentationAttribute? GetOverridePresentationAttribute(
            CrossViewModelRequest request,
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.Interfaces)] Type viewType);
    }
}
