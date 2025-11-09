namespace Nivaes.App.Cross.Droid
{
    public interface ICrossAndroidView
        : ICrossView
        , ICrossLayoutInflaterHolder
        , ICrossStartActivityForResult
        , ICrossBindingContextOwner
    {
    }

    public interface IMvxAndroidView<TViewModel>
        : ICrossAndroidView
        , ICrossView<TViewModel> where TViewModel : class, ICrossViewModel
    {
        CrossFluentBindingDescriptionSet<IMvxAndroidView<TViewModel>, TViewModel> CreateBindingSet();
    }
}
