// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;

namespace MvvmCross.Platforms.Android.Views
{
    using System;
    using System.Collections.Generic;
    using MvvmCross.Binding.BindingContext;
    using MvvmCross.Platforms.Android.Views.Base;
    using MvvmCross.Platforms.Android.Binding.BindingContext;
    using MvvmCross.Platforms.Android.Binding.Views;
    using Nivaes.App.Cross.ViewModels;
    using MvvmCross.Core;
    using Fragment = AndroidX.Fragment.App.Fragment;

    [Register("nivaes.app.MvxActivity")]
    public abstract class MvxActivity
        : MvxEventSourceActivity
        , IMvxAndroidView
    {
        protected View? View { get; set; }

        public IMvxBindingContext? BindingContext { get; set; }

        protected MvxActivity(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        protected MvxActivity()
        {
            BindingContext = new MvxAndroidBindingContext(this, this);
            this.AddEventListeners();
        }

        public object? DataContext
        {
            get { return BindingContext.DataContext; }
            set { BindingContext.DataContext = value; }
        }

        public IMvxViewModel ViewModel
        {
            get
            {
                return DataContext as IMvxViewModel;
            }
            set
            {
                DataContext = value;
                OnViewModelSet();
            }
        }

        public void MvxInternalStartActivityForResult(Intent intent, int requestCode)
        {
            StartActivityForResult(intent, requestCode);
        }

        public override void SetContentView(int layoutResId)
        {
            View = this.BindingInflate(layoutResId, null);

            SetContentView(View);
        }

        protected virtual void OnViewModelSet()
        {
        }

        protected override void AttachBaseContext(Context? @base)
        {
            if (this is IMvxSetupMonitor)
            {
                // Do not attach our inflater to splash screens.
                base.AttachBaseContext(@base);
                return;
            }
            base.AttachBaseContext(MvxContextWrapper.Wrap(@base, this));
        }

        private readonly List<WeakReference<Fragment>> _fragList = new List<WeakReference<Fragment>>();

        public override void OnAttachFragment(Fragment fragment)
        {
            base.OnAttachFragment(fragment);
            _fragList.Add(new WeakReference<Fragment>(fragment));
        }

        public List<Fragment> Fragments
        {
            get
            {
                var fragments = new List<Fragment>();
                foreach (var weakReference in _fragList)
                {
                    if (weakReference.TryGetTarget(out Fragment f))
                    {
                        if (f.IsVisible)
                            fragments.Add(f);
                    }
                }

                return fragments;
            }
        }

        protected override void OnCreate(Bundle? bundle)
        {
            base.OnCreate(bundle);
            _ = ViewModel?.ViewCreated().AsTask();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _ = ViewModel?.ViewDestroy(IsFinishing).AsTask();
        }

        protected override void OnStart()
        {
            base.OnStart();
            _ = ViewModel?.ViewAppearing().AsTask();
        }

        protected override void OnResume()
        {
            base.OnResume();
            _ = ViewModel?.ViewAppeared().AsTask();
        }

        protected override void OnPause()
        {
            base.OnPause();
            _= ViewModel?.ViewDisappearing().AsTask();
        }

        protected override void OnStop()
        {
            base.OnStop();
            _ = ViewModel?.ViewDisappeared().AsTask();
        }
    }

    public abstract class MvxActivity<TViewModel> : MvxActivity, IMvxAndroidView<TViewModel>
        where TViewModel : class, IMvxViewModel
    {
        public new TViewModel ViewModel
        {
            get { return (TViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }

        public MvxFluentBindingDescriptionSet<IMvxAndroidView<TViewModel>, TViewModel> CreateBindingSet()
        {
            return this.CreateBindingSet<IMvxAndroidView<TViewModel>, TViewModel>();
        }
    }
}
