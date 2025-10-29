namespace Nivaes.App.Cross
{
    public class CrossNavigateEventArgs : CrossCancelEventArgs, ICrossNavigateEventArgs
    {
        public CrossNavigateEventArgs(CrossNavigationMode mode, CancellationToken? cancellationToken = default) 
            : base(cancellationToken)
        {
            Mode = mode;
        }

        public CrossNavigateEventArgs(ICrossViewModel viewModel, CrossNavigationMode mode, CancellationToken? cancellationToken = default) 
            : this(mode, cancellationToken)
        {
            ViewModel = viewModel;
        }

        public CrossNavigationMode? Mode { get; set; }

        public ICrossViewModel? ViewModel { get; set; }
    }
}
