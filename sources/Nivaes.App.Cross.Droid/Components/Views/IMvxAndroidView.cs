namespace Nivaes.App.Cross.Droid
{
    using MvvmCross.Binding.BindingContext;
    using MvvmCross.Platforms.Android.Binding.Views;
    using MvvmCross.Platforms.Android.Views.Base;
    using MvvmCross.ViewModels;

    public interface IMvxAndroidView
        : ICrossView
        , IMvxLayoutInflaterHolder
        , IMvxStartActivityForResult
        , ICrossBindingContextOwner
    {
    }

    public interface IMvxAndroidView<TViewModel>
        : IMvxAndroidView
        , ICrossView<TViewModel> where TViewModel : class, ICrossViewModel
    {
        CrossFluentBindingDescriptionSet<IMvxAndroidView<TViewModel>, TViewModel> CreateBindingSet();
    }
}
