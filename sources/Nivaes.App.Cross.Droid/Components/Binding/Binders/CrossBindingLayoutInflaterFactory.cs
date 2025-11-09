namespace Nivaes.App.Cross.Droid
{
    using System.Diagnostics.CodeAnalysis;
    using Android.Content;
    using Android.Util;
    using Android.Views;
    using static Android.InputMethodServices.Keyboard;

    [RequiresUnreferencedCode("This class creates bindings which use reflection and may not be preserved by trimming.")]
    public class CrossBindingLayoutInflaterFactory
        : ICrossLayoutInflaterHolderFactory
    {
        private readonly object _source;

        private ICrossAndroidViewFactory? _androidViewFactory;
        private ICrossAndroidViewBinder? _binder;

        public CrossBindingLayoutInflaterFactory(object source)
        {
            _source = source;
        }

        protected virtual ICrossAndroidViewFactory? AndroidViewFactory => throw new NotImplementedException(); // _androidViewFactory ??= Mvx.IoCProvider?.Resolve<ICrossAndroidViewFactory>();

        protected virtual ICrossAndroidViewBinder? Binder => throw new NotImplementedException(); // _binder ??= Mvx.IoCProvider?.Resolve<ICrossAndroidViewBinderFactory>().Create(_source);

        public virtual IList<KeyValuePair<object, ICrossUpdateableBinding>>? CreatedBindings => Binder?.CreatedBindings;

        public virtual View? OnCreateView(View? parent, string name, Context context, IAttributeSet attrs)
        {
            if (name == "fragment")
            {
                // MvvmCross does not inflate Fragments - instead it returns null and lets Android inflate them.
                return null;
            }

            View? view = AndroidViewFactory?.CreateView(parent, name, context, attrs);
            return BindCreatedView(view, context, attrs);
        }

        public virtual View? BindCreatedView(View? view, Context context, IAttributeSet attrs)
        {
            if (view != null)
                Binder?.BindView(view, context, attrs);
            return view;
        }
    }
}
