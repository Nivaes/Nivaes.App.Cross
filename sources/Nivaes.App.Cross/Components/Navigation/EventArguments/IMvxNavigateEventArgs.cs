namespace MvvmCross.Navigation.EventArguments
{
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross;

    public interface IMvxNavigateEventArgs
    {
        bool Cancel { get; set; }
        NavigationMode Mode { get; set; }
        ICrossViewModel? ViewModel { get; set; }
    }
}