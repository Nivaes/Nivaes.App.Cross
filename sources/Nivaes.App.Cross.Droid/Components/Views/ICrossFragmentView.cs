namespace Nivaes.App.Cross.Droid
{
    public interface ICrossFragmentView
        : ICrossBindingContextOwner
        , ICrossView
    {
        string UniqueImmutableCacheTag { get; }
    }

    public interface ICrosFragmentView<TViewModel>
        : ICrossFragmentView
        , ICrossView<TViewModel> where TViewModel : class
        , ICrossViewModel
    {
        CrossFluentBindingDescriptionSet<ICrossFragmentView<TViewModel>, TViewModel> CreateBindingSet();
    }
}
