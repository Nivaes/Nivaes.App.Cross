namespace Nivaes.App.Cross
{
    public class CancelEventArgs
        : System.ComponentModel.CancelEventArgs
    {
        public CancelEventArgs(CancellationToken cancellationToken = default)
        {
            CancellationToken = cancellationToken;
            if (CancellationToken != default)
                CancellationToken.Register(Canceled);
        }
        protected CancellationToken CancellationToken { get; }

        protected virtual void Canceled()
        {
            Cancel = true;
        }
    }
}
