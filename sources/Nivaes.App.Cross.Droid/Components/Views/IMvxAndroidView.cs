namespace MvvmCross.Platforms.Android.Views
{
    using MvvmCross.Binding.BindingContext;
    using MvvmCross.Platforms.Android.Binding.Views;
    using MvvmCross.Platforms.Android.Views.Base;
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross;

    public interface IMvxAndroidView
        : ICrossView
        , IMvxLayoutInflaterHolder
        , IMvxStartActivityForResult
        , IMvxBindingContextOwner
    {
    }

    public interface IMvxAndroidView<TViewModel>
        : IMvxAndroidView
        , ICrossView<TViewModel> where TViewModel : class, IMvxViewModel
    {
        MvxFluentBindingDescriptionSet<IMvxAndroidView<TViewModel>, TViewModel> CreateBindingSet();
    }
}
