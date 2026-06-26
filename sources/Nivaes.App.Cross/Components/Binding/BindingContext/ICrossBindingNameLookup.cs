using System.Diagnostics.CodeAnalysis;

namespace Nivaes.App.Cross
{
    public interface ICrossBindingNameLookup
    {
        string? DefaultFor(
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)] Type type);
    }
}
