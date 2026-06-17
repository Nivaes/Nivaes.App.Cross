namespace Nivaes.App.Cross
{
    using System.Diagnostics.CodeAnalysis;

    [Obsolete("", true)]
    public interface ICrossTypeFinder
    {
        [return: DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicProperties)]
        Type? FindTypeOrNull(
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicProperties)] Type candidateType);
    }
}