﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Android.OS;
using Android.Runtime;

namespace MvvmCross.Platforms.Android.Views.Fragments
{
    using System;
    using MvvmCross.Binding.BindingContext;
    using MvvmCross.Platforms.Android.Views.Fragments.EventSource;
    using Nivaes.App.Cross.ViewModels;

    [Register("nivaes.app.MvxFragment")]
    public class MvxFragment
        : MvxEventSourceFragment
        , IMvxFragmentView
    {
        /// <summary>
        /// Create new instance of a Fragment
        /// </summary>
        /// <param name="bundle">Usually this would be MvxViewModelRequest serialized</param>
        /// <returns>Returns an instance of a MvxFragment</returns>
        public static MvxFragment NewInstance(Bundle bundle)
        {
            // Setting Arguments needs to happen before Fragment is attached
            // to Activity. Arguments are persisted when Fragment is recreated!
            var fragment = new MvxFragment { Arguments = bundle };

            return fragment;
        }

        protected MvxFragment(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        protected MvxFragment()
        {
            this.AddEventListeners();
        }

        public IMvxBindingContext? BindingContext { get; set; }

        private object? mDataContext;

        public object? DataContext
        {
            get => mDataContext;
            set
            {
                mDataContext = value;
                if (BindingContext != null)
                    BindingContext.DataContext = value;
            }
        }

        public virtual IMvxViewModel ViewModel
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

        public virtual void OnViewModelSet()
        {
        }

        public string UniqueImmutableCacheTag => Tag;

        public override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            _ = ViewModel?.ViewCreated().AsTask();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            _ = ViewModel?.ViewDestroy(viewFinishing: IsRemoving || Activity == null || Activity.IsFinishing).AsTask();
        }

        public override void OnStart()
        {
            base.OnStart();
            _ = ViewModel?.ViewAppearing().AsTask();
        }

        public override void OnResume()
        {
            base.OnResume();
            _ = ViewModel?.ViewAppeared().AsTask();
        }

        public override void OnPause()
        {
            base.OnPause();
            _ = ViewModel?.ViewDisappearing().AsTask();
        }

        public override void OnStop()
        {
            base.OnStop();
            _ = ViewModel?.ViewDisappeared().AsTask();
        }
    }

    public abstract class MvxFragment<TViewModel>
        : MvxFragment, IMvxFragmentView<TViewModel> 
        where TViewModel : class, IMvxViewModel
    {
        protected MvxFragment()
        {
        }

        protected MvxFragment(IntPtr javaReference, JniHandleOwnership transfer): base(javaReference, transfer)
        {
        }

        public new TViewModel ViewModel
        {
            get { return (TViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }

        public MvxFluentBindingDescriptionSet<IMvxFragmentView<TViewModel>, TViewModel> CreateBindingSet()
        {
            return this.CreateBindingSet<IMvxFragmentView<TViewModel>, TViewModel>();
        }
    }
}
