namespace Nivaes.App.Cross.Tvos
{
    using MvvmCross.ViewModels;

    public interface IMvxCurrentRequest
    {
        CrossViewModelRequest CurrentRequest { get; }
    }
}
