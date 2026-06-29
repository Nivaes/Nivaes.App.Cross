using System.Diagnostics.CodeAnalysis;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Object = Java.Lang.Object;

namespace Nivaes.App.Cross.Droid
{
    public static class MvxLayoutInflaterCompat
    {
        internal class FactoryWrapper : Object, LayoutInflater.IFactory
        {
            protected readonly IMvxLayoutInflaterFactory? DelegateFactory;

            [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(FactoryWrapper))]
            public FactoryWrapper(IntPtr handle, JniHandleOwnership ownership)
                : base(handle, ownership)
            {
            }

            public FactoryWrapper(IMvxLayoutInflaterFactory delegateFactory)
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

            public FactoryWrapper2(IMvxLayoutInflaterFactory delegateFactory)
                : base(delegateFactory)
            {
            }

            public View? OnCreateView(View? parent, string name, Context context, IAttributeSet attrs)
            {
                return DelegateFactory.OnCreateView(parent, name, context, attrs);
            }
        }

        public static void SetFactory(LayoutInflater layoutInflater, IMvxLayoutInflaterFactory? factory)
        {
            layoutInflater.Factory2 = factory != null ? new FactoryWrapper2(factory) : null;
        }
    }
}
