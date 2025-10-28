namespace Nivaes.App.Cross
{
    public class ChangePresentationEventArgs 
        : CancelEventArgs
    {
        public ChangePresentationEventArgs(CancellationToken cancellationToken = default) : base(cancellationToken)
        {
        }

        //public ChangePresentationEventArgs(PresentationHint hint, CancellationToken cancellationToken = default) : this(cancellationToken)
        //{
        //    Hint = hint;
        //}

        //public PresentationHint Hint { get; set; }

        public bool? Result { get; set; }
    }
}
