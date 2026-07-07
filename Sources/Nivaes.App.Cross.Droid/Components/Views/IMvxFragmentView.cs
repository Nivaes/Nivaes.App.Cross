namespace Nivaes.App.Cross.Droid
{
    public interface IMvxFragmentView
        : ICrossBindingContextOwner, ICrossView
    {
        string UniqueImmutableCacheTag { get; }
    }

    public interface IMvxFragmentView<TViewModel>
        : IMvxFragmentView, ICrossView<TViewModel>
        where TViewModel : ICrossViewModel
    {
        CrossFluentBindingDescriptionSet<IMvxFragmentView<TViewModel>, TViewModel> CreateBindingSet();
    }
}
