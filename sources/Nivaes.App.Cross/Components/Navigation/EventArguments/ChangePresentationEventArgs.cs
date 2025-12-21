namespace Nivaes.App.Cross
{
    using System.Threading;
    using MvvmCross.ViewModels;

    public class ChangePresentationEventArgs 
        : CrossCancelEventArgs
    {
        public ChangePresentationEventArgs(CancellationToken cancellationToken = default)
            : base(cancellationToken)
        {
        }

        public ChangePresentationEventArgs(MvxPresentationHint? hint, CancellationToken cancellationToken = default)
            : this(cancellationToken)
        {
            Hint = hint;
        }

        public MvxPresentationHint? Hint { get; set; }

        public bool? Result { get; set; }
    }
}
