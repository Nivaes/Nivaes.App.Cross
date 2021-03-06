// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Android.Views;

namespace MvvmCross.Platforms.Android.Binding.BindingContext
{
    using System;
    using MvvmCross.Binding.BindingContext;

    public static class MvxBindingContextOwnerExtensions
    {
        public static View BindingInflate(this IMvxBindingContextOwner owner, int resourceId, ViewGroup? viewGroup)
        {
            if (owner == null) throw new ArgumentNullException(nameof(owner));

            var context = (IMvxAndroidBindingContext)owner.BindingContext;
            return context.BindingInflate(resourceId, viewGroup);
        }

        public static View BindingInflate(this IMvxBindingContextOwner owner, int resourceId, ViewGroup? viewGroup, bool attachToParent)
        {
            if (owner == null) throw new ArgumentNullException(nameof(owner));

            var context = (IMvxAndroidBindingContext)owner.BindingContext;
            return context.BindingInflate(resourceId, viewGroup, attachToParent);
        }
    }
}
