namespace Nivaes.App.Cross
{
    [Obsolete()]
    public abstract class CrossApplicableTo<T>
        : CrossApplicable,
          ICrossApplicableTo<T>
        where T : notnull
    {
        [System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
        public virtual void ApplyTo(T what)
        {
            SuppressFinalizer();
        }
    }
}
