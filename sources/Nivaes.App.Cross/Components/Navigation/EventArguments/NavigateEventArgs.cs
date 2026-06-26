namespace Nivaes.App.Cross
{
    using System.Threading;

    public enum NavigationMode
    {
        None,
        Show,
        Close
    }

    public class CrossNavigateEventArgs
        : CrossCancelEventArgs, ICrossNavigateEventArgs
    {
        public CrossNavigateEventArgs(NavigationMode mode, CancellationToken cancellationToken = default)
            : base(cancellationToken)
        {
            Mode = mode;
        }

        public CrossNavigateEventArgs(ICrossViewModel viewModel, NavigationMode mode, CancellationToken cancellationToken = default)
            : this(mode, cancellationToken)
        {
            ViewModel = viewModel;
        }

        public NavigationMode Mode { get; set; }
        public ICrossViewModel? ViewModel { get; set; }
    }
}
