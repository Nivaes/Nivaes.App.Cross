// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;

namespace MvvmCross.Platforms.Android.Binding.Binders
{
    using System;
    using Java.Lang;
    using Java.Lang.Reflect;
    using Nivaes.App.Cross.Logging;
    using Object = Java.Lang.Object;

    public static class MvxLayoutInflaterCompat
    {
        private static readonly int SdkInt = (int)Build.VERSION.SdkInt;
        private static Field? mLayoutInflaterFactory2Field;
        private static bool mCheckedField;

        internal class FactoryWrapper : Object, LayoutInflater.IFactory
        {
            protected readonly IMvxLayoutInflaterFactory? DelegateFactory;

            public FactoryWrapper(IntPtr handle, JniHandleOwnership ownership)
                : base(handle, ownership)
            {
            }

            public FactoryWrapper(IMvxLayoutInflaterFactory delegateFactory)
            {
                DelegateFactory = delegateFactory;
            }

            public View OnCreateView(string name, Context context, IAttributeSet attrs)
            {
                return DelegateFactory.OnCreateView(null, name, context, attrs);
            }
        }

        internal class FactoryWrapper2 : FactoryWrapper, LayoutInflater.IFactory2
        {
            public FactoryWrapper2(IntPtr handle, JniHandleOwnership ownership)
                : base(handle, ownership)
            {
            }

            public FactoryWrapper2(IMvxLayoutInflaterFactory delegateFactory)
                : base(delegateFactory)
            {
            }

            public View OnCreateView(View parent, string name, Context context, IAttributeSet attrs)
            {
                return DelegateFactory.OnCreateView(parent, name, context, attrs);
            }
        }

        public static void SetFactory(LayoutInflater layoutInflater, IMvxLayoutInflaterFactory factory)
        {
            if (SdkInt >= 21)
            {
                layoutInflater.Factory2 = factory != null ? new FactoryWrapper2(factory) : null;
            }
            else if (SdkInt >= 11)
            {
                var factory2 = factory != null ? new FactoryWrapper2(factory) : null;
                layoutInflater.Factory2 = factory2;

                LayoutInflater.IFactory f = layoutInflater.Factory;
                var f2 = f as LayoutInflater.IFactory2;

                // The merged factory is now set to Factory, but not Factory2 (pre-v21).
                // We will now try and force set the merged factory to mFactory2
                ForceSetFactory2(layoutInflater, f2 ?? factory2);
            }
            else
            {
                layoutInflater.Factory = factory != null ? new FactoryWrapper(factory) : null;
            }
        }

        // Workaround from Support.v4 v22.1.1 library:
        //
        // For APIs >= 11 && < 21, there was a framework bug that prevented a LayoutInflater's
        // Factory2 from being merged properly if set after a cloneInContext from a LayoutInflater
        // that already had a Factory2 registered. We work around that bug here. If we can't we
        // log an error.
        private static void ForceSetFactory2(LayoutInflater inflater, LayoutInflater.IFactory2 factory)
        {
            if (!mCheckedField)
            {
                //try
                //{
                    Class layoutInflaterClass = Class.FromType(typeof(LayoutInflater));
                    mLayoutInflaterFactory2Field = layoutInflaterClass.GetDeclaredField("mFactory2");
                    mLayoutInflaterFactory2Field.Accessible = true;
                //}
                //catch (NoSuchFieldException)
                //{
                //    MvxLog.Instance?.Error(
                //        "ForceSetFactory2 Could not find field 'mFactory2' on class {0}; inflation may have unexpected results.",
                //        Class.FromType(typeof(LayoutInflater)).Name);
                //}
                mCheckedField = true;
            }

            if (mLayoutInflaterFactory2Field != null)
            {
                //try
                //{
                    mLayoutInflaterFactory2Field.Set(inflater, (Object)factory);
                //}
                //catch (IllegalAccessException)
                //{
                //    MvxLog.Instance?.Error("ForceSetFactory2 could not set the Factory2 on LayoutInflater {0} ; inflation may have unexpected results.", inflater);
                //}
            }
        }
    }
}
