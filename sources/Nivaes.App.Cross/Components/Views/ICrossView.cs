namespace Nivaes.App.Cross
{
    using MvvmCross.Base;

    public interface ICrossView
        : IMvxDataConsumer
    {
        ICrossViewModel? ViewModel { get; set; }
    }

    public interface ICrossView<TViewModel>
        : ICrossView where TViewModel : class, ICrossViewModel
    {
        new TViewModel? ViewModel { get; set; }
    }
}
