using System.Diagnostics.CodeAnalysis;

namespace Nivaes.App.Cross;

public interface ICrossViewFinder
{
    [return: DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.Interfaces)]
    Type GetViewType(Type viewModelType);
}
