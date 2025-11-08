namespace Nivaes.App.Cross.Droid
{
    using Android.Content;
    using Android.Runtime;
    using Android.Util;
    using Android.Views;
    using Object = Java.Lang.Object;

    public static class CrossLayoutInflaterCompat
    {
        internal class FactoryWrapper : Object, LayoutInflater.IFactory
        {
            protected readonly ICrossLayoutInflaterFactory DelegateFactory;

            [Preserve(Conditional = true)]
#pragma warning disable 8618
            public FactoryWrapper(IntPtr handle, JniHandleOwnership ownership)
#pragma warning restore 8618
                : base(handle, ownership)
            {
            }

            public FactoryWrapper(ICrossLayoutInflaterFactory delegateFactory)
            {
                DelegateFactory = delegateFactory;
            }

            public View? OnCreateView(string name, Context context, IAttributeSet attrs)
            {
                return DelegateFactory.OnCreateView(null, name, context, attrs);
            }
        }

        internal class FactoryWrapper2 : FactoryWrapper, LayoutInflater.IFactory2
        {
            [Preserve(Conditional = true)]
            public FactoryWrapper2(IntPtr handle, JniHandleOwnership ownership)
                : base(handle, ownership)
            {
            }

            public FactoryWrapper2(ICrossLayoutInflaterFactory delegateFactory)
                : base(delegateFactory)
            {
            }

            public View? OnCreateView(View? parent, string name, Context context, IAttributeSet attrs)
            {
                return DelegateFactory.OnCreateView(parent, name, context, attrs);
            }
        }

        public static void SetFactory(LayoutInflater layoutInflater, ICrossLayoutInflaterFactory? factory)
        {
            layoutInflater.Factory2 = factory != null ? new FactoryWrapper2(factory) : null;
        }
    }
}
