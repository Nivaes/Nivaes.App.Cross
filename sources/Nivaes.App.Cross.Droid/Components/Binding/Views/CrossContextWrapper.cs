namespace Nivaes.App.Cross.Droid
{
    using Android.Content;
    using Android.Runtime;
    using Android.Views;
    using Object = Java.Lang.Object;

    [Register("Nivaes.App.Cross.Droid.CrossContextWrapper")]
    public class CrossContextWrapper 
        : ContextWrapper
    {
        private LayoutInflater? _inflater;
        private readonly ICrossBindingContextOwner _bindingContextOwner;

        public static ContextWrapper Wrap(Context @base, ICrossBindingContextOwner bindingContextOwner)
        {
            return new CrossContextWrapper(@base, bindingContextOwner);
        }

        protected CrossContextWrapper(Context context, ICrossBindingContextOwner bindingContextOwner)
            : base(context)
        {
            if (bindingContextOwner == null)
                throw new InvalidOperationException("Wrapper can only be set on ICrossBindingContextOwner");

            _bindingContextOwner = bindingContextOwner;
        }

        public override Object GetSystemService(string name)
        {
            if (string.Equals(name, LayoutInflaterService, StringComparison.InvariantCulture))
            {
                return _inflater ??=
                    new CrossLayoutInflater(LayoutInflater.From(BaseContext), this, null, false);
            }

            return base.GetSystemService(name);
        }
    }
}
