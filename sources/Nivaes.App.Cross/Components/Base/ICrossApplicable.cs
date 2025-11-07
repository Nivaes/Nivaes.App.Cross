namespace Nivaes.App.Cross
{
    using System.Diagnostics.CodeAnalysis;

    public interface ICrossApplicable
    {
        [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
        void Apply();
    }
}
