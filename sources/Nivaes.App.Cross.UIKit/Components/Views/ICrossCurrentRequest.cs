namespace MvvmCross.Platforms.Ios.Views
{
    using Nivaes.App.Cross;

    public interface ICrossCurrentRequest
    {
        CrossViewModelRequest CurrentRequest { get; }
    }
}
