namespace Nivaes.App.Cross
{
    public abstract class CrossApplicableTo<T>
        : CrossApplicable,
          IMvxApplicableTo<T>
        where T : notnull
    {
        [System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
        public virtual void ApplyTo(T what)
        {
            SuppressFinalizer();
        }
    }
}
