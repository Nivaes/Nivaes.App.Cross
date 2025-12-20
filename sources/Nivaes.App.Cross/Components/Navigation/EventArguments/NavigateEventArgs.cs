namespace MvvmCross.Navigation.EventArguments
{
    using System.ComponentModel;
    using System.Threading;
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross;

    public enum NavigationMode
    {
        None,
        Show,
        Close
    }

    public class MvxNavigateEventArgs 
        : MvxCancelEventArgs, IMvxNavigateEventArgs
    {
        public MvxNavigateEventArgs(NavigationMode mode, CancellationToken cancellationToken = default)
            : base(cancellationToken)
        {
            Mode = mode;
        }

        public MvxNavigateEventArgs(ICrossViewModel viewModel, NavigationMode mode, CancellationToken cancellationToken = default)
            : this(mode, cancellationToken)
        {
            ViewModel = viewModel;
        }

        public NavigationMode Mode { get; set; }
        public ICrossViewModel? ViewModel { get; set; }
    }
}
