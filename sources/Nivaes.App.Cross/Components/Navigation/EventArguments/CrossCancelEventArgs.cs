namespace Nivaes.App.Cross
{
    using System.ComponentModel;

    public class CrossCancelEventArgs 
        : CancelEventArgs
    {
        public CrossCancelEventArgs(CancellationToken? cancellationToken = default)
        {
            CancellationToken = cancellationToken;
            CancellationToken?.Register(Canceled);
        }
        protected CancellationToken? CancellationToken { get; }

        protected virtual void Canceled()
        {
            Cancel = true;
        }
    }
}
