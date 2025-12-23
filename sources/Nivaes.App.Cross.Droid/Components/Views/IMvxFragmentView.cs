namespace Nivaes.App.Cross.Droid
{
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
