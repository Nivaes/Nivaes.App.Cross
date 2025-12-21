namespace MvvmCross.Platforms.Ios.Binding.Views
{
    using MvvmCross.Base;
    using MvvmCross.Binding.BindingContext;
    using Nivaes.App.Cross;

    public interface IMvxBindable
        : IMvxBindingContextOwner
        , ICrossDataConsumer
    {
    }
}
