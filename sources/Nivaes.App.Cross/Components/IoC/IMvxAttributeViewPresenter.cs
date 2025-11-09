namespace MvvmCross.Presenters
{
    using System.Diagnostics.CodeAnalysis;

    public interface IMvxAttributeViewPresenter : IMvxViewPresenter
    {
        //IMvxViewModelTypeFinder? ViewModelTypeFinder { get; }
        //IMvxViewsContainer? ViewsContainer { get; }
        //IDictionary<Type, MvxPresentationAttributeAction>? AttributeTypesToActionsDictionary { get; }
        //void RegisterAttributeTypes();

        ////TODO: Maybe move those to helper class
        //MvxBasePresentationAttribute GetPresentationAttribute(MvxViewModelRequest request);
        //MvxBasePresentationAttribute CreatePresentationAttribute(Type viewModelType, Type viewType);
        //MvxBasePresentationAttribute? GetOverridePresentationAttribute(
        //    MvxViewModelRequest request,
        //    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.Interfaces)] Type viewType);
    }
}