namespace Nivaes.App.Cross
{
    using System.Diagnostics.CodeAnalysis;

    public interface ICrossViewFinder
    {
        [return: DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.Interfaces)]
        Type? GetViewType(Type? viewModelType);
    }
}
