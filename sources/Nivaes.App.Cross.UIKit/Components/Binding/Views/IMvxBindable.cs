namespace Nivaes.App.Cross.UIKit
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
