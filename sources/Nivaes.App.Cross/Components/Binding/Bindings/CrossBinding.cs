namespace Nivaes.App.Cross
{
    using System.Diagnostics.CodeAnalysis;

    public abstract class CrossBinding : ICrossBinding
    {
        [UnconditionalSuppressMessage("Trimming", "IL2026", Justification = "Bindings inherently use reflection. This is by design and callers are warned through derived class usage.")]
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
        protected virtual void Dispose(bool isDisposing)
        {
            // nothing to do in this base class
        }
    }
}