namespace MvvmCross.Platforms.Android.Views
{
    using MvvmCross.Binding.BindingContext;
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross;

    public interface IMvxFragmentView
        : IMvxBindingContextOwner
        , ICrossView
    {
        string UniqueImmutableCacheTag { get; }
    }

    public interface IMvxFragmentView<TViewModel>
        : IMvxFragmentView
        , ICrossView<TViewModel> where TViewModel : class
        , ICrossViewModel
    {
        MvxFluentBindingDescriptionSet<IMvxFragmentView<TViewModel>, TViewModel> CreateBindingSet();
    }
}
