namespace MvvmCross.Platforms.Tvos.Views
{
    using Nivaes.App.Cross;

    public interface IMvxModalTvosView
        : ICrossView
    {
    }

    public interface IMvxModalTvosView<TViewModel>
        : IMvxModalTvosView
        , ICrossView<TViewModel> where TViewModel : class, ICrossViewModel
    {
    }
}
