// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using Android.Content;
using Android.Runtime;
using Android.Views;
using MvvmCross.Binding.BindingContext;
using Object = Java.Lang.Object;

namespace Nivaes.App.Cross.Droid
{
    [Register("mvvmcross.platforms.android.binding.views.MvxContextWrapper")]
    public class CrossContextWrapper : ContextWrapper
    {
        private LayoutInflater _inflater;
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
