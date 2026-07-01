namespace Nivaes.App.Cross.Droid;

using System.Diagnostics.CodeAnalysis;

public interface IMvxViewTypeResolver
{
    [return: DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
    Type? Resolve(string tagName);
}
