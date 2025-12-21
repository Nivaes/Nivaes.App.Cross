namespace Nivaes.App.Cross
{
    using System;
    using Microsoft.Extensions.Logging;
    using MvvmCross.Logging;

    public abstract class CrossApplicable
        : ICrossApplicable
    {
        private bool _finalizerSuppressed;

        ~CrossApplicable()
        {
            MvxLogHost.Default?.Log(LogLevel.Trace, "Finaliser called on {0} - suggests that  Apply() was never called", GetType().Name);
        }

        protected void SuppressFinalizer()
        {
            if (_finalizerSuppressed)
                return;

            _finalizerSuppressed = true;
#pragma warning disable CA1816 // Dispose methods should call SuppressFinalize
#pragma warning disable S3971 // "GC.SuppressFinalize" should not be called
            GC.SuppressFinalize(this);
#pragma warning restore S3971 // "GC.SuppressFinalize" should not be called
#pragma warning restore CA1816 // Dispose methods should call SuppressFinalize
        }

        [System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
        public virtual void Apply()
        {
            SuppressFinalizer();
        }
    }
}
