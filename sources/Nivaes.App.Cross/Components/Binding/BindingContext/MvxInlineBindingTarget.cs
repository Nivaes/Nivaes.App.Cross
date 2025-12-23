namespace Nivaes.App.Cross
{
    public class MvxInlineBindingTarget<TViewModel>
    {
        public MvxInlineBindingTarget(IMvxBindingContextOwner bindingContextOwner)
        {
            BindingContextOwner = bindingContextOwner;
        }

        public IMvxBindingContextOwner BindingContextOwner { get; private set; }
    }
}
