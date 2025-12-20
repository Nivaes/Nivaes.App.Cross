namespace Nivaes.App.Cross
{
    using System.Diagnostics.CodeAnalysis;

    [Obsolete()]
    public interface ICrossApplicableTo
    {
        [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
        void ApplyTo(object what);
    }

    [Obsolete()]
    public interface ICrossApplicableTo<in T>
        where T : notnull
    {
        [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
        void ApplyTo(T what);
    }
}
