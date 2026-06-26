namespace Nivaes.App.Cross
{
    using System.Threading;

    public class ChangePresentationEventArgs
        : CrossCancelEventArgs
    {
        public ChangePresentationEventArgs(CancellationToken cancellationToken = default)
            : base(cancellationToken)
        {
        }

        public ChangePresentationEventArgs(CrossPresentationHint? hint, CancellationToken cancellationToken = default)
            : this(cancellationToken)
        {
            Hint = hint;
        }

        public CrossPresentationHint? Hint { get; set; }

        public bool? Result { get; set; }
    }
}
