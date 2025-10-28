namespace Nivaes.App.Cross
{
    public class NavigateEventArgs : CrossCancelEventArgs, INavigateEventArgs
    {
        public NavigateEventArgs(NavigationMode mode, CancellationToken cancellationToken = default) 
            : base(cancellationToken)
        {
            Mode = mode;
        }

        public NavigateEventArgs(IViewModel viewModel, NavigationMode mode, CancellationToken cancellationToken = default) 
            : this(mode, cancellationToken)
        {
            ViewModel = viewModel;
        }

        public NavigationMode? Mode { get; set; }

        public IViewModel? ViewModel { get; set; }
    }
}
