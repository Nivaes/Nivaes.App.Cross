namespace Nivaes.App.Cross.Droid
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Android.Content;
    using Android.Runtime;
    using Android.Views;
    using MvvmCross.Platforms.Android.Binding.Views;
    using Nivaes.App.Cross;
    using Object = Java.Lang.Object;

    [Register("mvvmcross.platforms.android.binding.views.MvxContextWrapper")]
    [RequiresUnreferencedCode("MvxBindings require unreferenced code")]
    public class MvxContextWrapper : ContextWrapper
    {
        private LayoutInflater _inflater;
        private readonly ICrossBindingContextOwner _bindingContextOwner;

        public static ContextWrapper Wrap(Context @base, ICrossBindingContextOwner bindingContextOwner)
        {
            return new MvxContextWrapper(@base, bindingContextOwner);
        }

        protected MvxContextWrapper(Context context, ICrossBindingContextOwner bindingContextOwner)
            : base(context)
        {
            if (bindingContextOwner == null)
                throw new InvalidOperationException("Wrapper can only be set on IMvxBindingContextOwner");

            _bindingContextOwner = bindingContextOwner;
        }

        public override Object GetSystemService(string name)
        {
            if (string.Equals(name, LayoutInflaterService, StringComparison.InvariantCulture))
            {
                return _inflater ??=
                    new MvxLayoutInflater(LayoutInflater.From(BaseContext), this, null, false);
            }

            return base.GetSystemService(name);
        }
    }
}
