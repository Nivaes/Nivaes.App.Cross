namespace Nivaes.App.Cross
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    public interface IMvxBindingNameLookup
    {
        string DefaultFor(
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)] Type type);
    }
}
