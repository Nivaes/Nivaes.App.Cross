namespace Nivaes.App.Cross.Droid;

public interface IMvxAndroidView
    : ICrossView
    , IMvxLayoutInflaterHolder
    , IMvxStartActivityForResult
    , ICrossBindingContextOwner
{
}

public interface IMvxAndroidView<TViewModel>
    : IMvxAndroidView
    , ICrossView<TViewModel> where TViewModel : ICrossViewModel
{
    CrossFluentBindingDescriptionSet<IMvxAndroidView<TViewModel>, TViewModel> CreateBindingSet();
}
