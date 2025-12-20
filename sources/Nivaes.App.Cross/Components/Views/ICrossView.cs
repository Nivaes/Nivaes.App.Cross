namespace Nivaes.App.Cross
{
    using MvvmCross.Base;
    using MvvmCross.ViewModels;

    public interface ICrossView
        : IMvxDataConsumer
    {
        IMvxViewModel? ViewModel { get; set; }
    }

    public interface ICrossView<TViewModel>
        : ICrossView where TViewModel : class, IMvxViewModel
    {
        new TViewModel? ViewModel { get; set; }
    }
}
