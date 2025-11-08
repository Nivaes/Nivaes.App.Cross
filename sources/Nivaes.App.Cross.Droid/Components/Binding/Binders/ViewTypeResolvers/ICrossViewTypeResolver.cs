namespace Nivaes.App.Cross.Droid
{
    using System.Diagnostics.CodeAnalysis;

    public interface ICrossViewTypeResolver
    {
        [return: DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
        Type Resolve(string tagName);
    }
}
