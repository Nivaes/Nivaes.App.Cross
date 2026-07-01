namespace Nivaes.App.Cross
{
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Base class for all cross bindings.
    /// </summary>
    public abstract class CrossBinding
        : ICrossBinding
    {
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