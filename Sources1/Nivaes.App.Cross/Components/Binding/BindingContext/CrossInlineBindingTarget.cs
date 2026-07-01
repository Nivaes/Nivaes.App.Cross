namespace Nivaes.App.Cross
{
    public class CrossInlineBindingTarget<TViewModel>
    {
        public CrossInlineBindingTarget(ICrossBindingContextOwner bindingContextOwner)
        {
            BindingContextOwner = bindingContextOwner;
        }

        public ICrossBindingContextOwner BindingContextOwner { get; private set; }
    }
}
