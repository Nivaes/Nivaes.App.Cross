namespace Nivaes.App.Cross
{
    using System.Threading;

    public class CrossChangePresentationEventArgs 
        : CrossCancelEventArgs
    {
        public CrossChangePresentationEventArgs(CancellationToken cancellationToken = default) : base(cancellationToken)
        {
        }

        public CrossChangePresentationEventArgs(CrossPresentationHint hint, CancellationToken cancellationToken = default) : this(cancellationToken)
        {
            Hint = hint;
        }

        public CrossPresentationHint? Hint { get; set; }

        public bool? Result { get; set; }
    }
}
