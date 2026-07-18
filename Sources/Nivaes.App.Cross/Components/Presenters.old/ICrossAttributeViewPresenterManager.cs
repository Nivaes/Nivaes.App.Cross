namespace Nivaes.App.Cross
{
    using System.Diagnostics.CodeAnalysis;

    public interface ICrossAttributeViewPresenterManager : ICrossViewPresenterManager
    {

        //TODO: Maybe move those to helper class
        BasePresentationAttribute GetPresentationAttribute(CrossViewModelRequest request);

        BasePresentationAttribute CreatePresentationAttribute(
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type? viewModelType,
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type? viewType);

    }
}
