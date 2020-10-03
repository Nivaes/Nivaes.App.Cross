// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Android.Content;
using Android.Content.Res;
using Android.Util;
using Android.Views;

namespace MvvmCross.Platforms.Android.Binding.Binders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using MvvmCross.Exceptions;
    using MvvmCross.Binding;
    using MvvmCross.Binding.Binders;
    using MvvmCross.Binding.Bindings;
    using MvvmCross.Platforms.Android.Binding.ResourceHelpers;

    public class MvxAndroidViewBinder
        : IMvxAndroidViewBinder
    {
        private readonly List<KeyValuePair<object, IMvxUpdateableBinding>> mViewBindings = new List<KeyValuePair<object, IMvxUpdateableBinding>>();
        private readonly Lazy<IMvxAndroidBindingResource> mvxAndroidBindingResource = new Lazy<IMvxAndroidBindingResource>(() => Mvx.IoCProvider.GetSingleton<IMvxAndroidBindingResource>());

        private readonly object mSource;
        private IMvxBinder? mBinder;

        public MvxAndroidViewBinder(object source)
        {
            mSource = source;
        }

        protected IMvxBinder Binder => mBinder ??= Mvx.IoCProvider.Resolve<IMvxBinder>();

        public IList<KeyValuePair<object, IMvxUpdateableBinding>> CreatedBindings => mViewBindings;

        public virtual void BindView(View view, Context context, IAttributeSet attrs)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            using (var typedArray = context.ObtainStyledAttributes(attrs, mvxAndroidBindingResource.Value.BindingStylableGroupId.ToArray()))
            {
                int numStyles = typedArray.IndexCount;
                for (var i = 0; i < numStyles; ++i)
                {
                    var attributeId = typedArray.GetIndex(i);

                    if (attributeId == mvxAndroidBindingResource.Value.BindingBindId)
                    {
                        ApplyBindingsFromAttribute(view, typedArray, attributeId);
                    }
                    else if (attributeId == mvxAndroidBindingResource.Value.BindingLangId)
                    {
                        ApplyLanguageBindingsFromAttribute(view, typedArray, attributeId);
                    }
                }
                typedArray.Recycle();
            }
        }

        private void ApplyBindingsFromAttribute(View view, TypedArray typedArray, int attributeId)
        {
            try
            {
                var bindingText = typedArray.GetString(attributeId);
                var newBindings = Binder.Bind(mSource, view, bindingText!);
                StoreBindings(view, newBindings);
            }
            catch (Exception exception)
            {
                MvxBindingLog.Error( "Exception thrown during the view binding {0}",
                                      exception.ToLongString());
            }
        }

        private void StoreBindings(View view, IEnumerable<IMvxUpdateableBinding> newBindings)
        {
            if (newBindings != null)
            {
                mViewBindings.AddRange(newBindings.Select(b => new KeyValuePair<object, IMvxUpdateableBinding>(view, b)));
            }
        }

        private void ApplyLanguageBindingsFromAttribute(View view, TypedArray typedArray, int attributeId)
        {
            try
            {
                var bindingText = typedArray.GetString(attributeId);
                var newBindings = Binder.LanguageBind(mSource, view, bindingText!);
                StoreBindings(view, newBindings);
            }
            catch (Exception exception)
            {
                MvxBindingLog.Error( "Exception thrown during the view language binding {0}",
                                      exception.ToLongString());
                throw;
            }
        }
    }
}
