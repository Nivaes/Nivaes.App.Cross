namespace MvvmCross.Platforms.Mac.Views
{
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross;

    public interface IMvxCurrentRequest
    {
        CrossViewModelRequest CurrentRequest { get; }
    }
}
