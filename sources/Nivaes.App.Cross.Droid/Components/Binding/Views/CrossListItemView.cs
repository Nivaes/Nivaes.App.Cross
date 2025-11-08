// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using Android.Content;
using Android.Runtime;
using Android.Views;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using Object = Java.Lang.Object;

namespace Nivaes.App.Cross.Droid
{
    [Register("mvvmcross.platforms.android.binding.views.MvxListItemView")]
    public class CrossListItemView : Object, ICrossListItemView,
        ICrossBindingContextOwner, View.IOnAttachStateChangeListener
    {
        private readonly ICrossAndroidBindingContext _bindingContext;
        private View _content;
        private object _cachedDataContext;
        private bool _isAttachedToWindow;

        public CrossListItemView(Context context,
            ICrossLayoutInflaterHolder layoutInflaterHolder, object dataContext,
            ViewGroup parent, int templateId)
        {
            _bindingContext = new CrossAndroidBindingContext(context, layoutInflaterHolder, dataContext);
            TemplateId = templateId;
            Content = _bindingContext.BindingInflate(templateId, parent, false);
        }

        public void OnViewAttachedToWindow(View attachedView)
        {
            _isAttachedToWindow = true;

            if (_cachedDataContext != null && DataContext == null)
                DataContext = _cachedDataContext;
        }

        public void OnViewDetachedFromWindow(View detachedView)
        {
            _cachedDataContext = DataContext;
            DataContext = null;
            _isAttachedToWindow = false;
        }

        public IMvxBindingContext BindingContext
        {
            get => _bindingContext;
            set => throw new NotImplementedException("BindingContext is readonly in the list item");
        }

        public View Content
        {
            get => _content;
            set
            {
                _content = value;
                _content.AddOnAttachStateChangeListener(this);
            }
        }

        public virtual object DataContext
        {
            get => _bindingContext.DataContext;
            set
            {
                if (_isAttachedToWindow)
                {
                    _bindingContext.DataContext = value;
                }
                else
                {
                    _cachedDataContext = value;
                    _bindingContext.DataContext = null;
                }
            }
        }

        public int TemplateId { get; protected set; }
    }
}
