namespace Nivaes.App.Cross.UIKit.TvOS
{
    using MvvmCross.ViewModels;

    public interface IMvxCurrentRequest
    {
        CrossViewModelRequest CurrentRequest { get; }
    }
}
