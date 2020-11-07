// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.Platforms.Ios.Views
{
    using System;
    using Foundation;
    using MvvmCross.Binding.BindingContext;
    using MvvmCross.Platforms.Ios.Views.Base;
    using MvvmCross.ViewModels;
    using UIKit;

    public class MvxViewController
        : MvxEventSourceViewController, IMvxIosView
    {
        public MvxViewController()
            : base()
        {
            this.AdaptForBinding();
        }

        public MvxViewController(NSCoder coder)
            : base(coder)
        {
            this.AdaptForBinding();
        }

        protected MvxViewController(NSObjectFlag t)
            : base(t)
        {
            this.AdaptForBinding();
        }

        protected internal MvxViewController(IntPtr handle)
            : base(handle)
        {
            this.AdaptForBinding();
        }

        public MvxViewController(string? nibName, NSBundle? bundle)
            : base(nibName, bundle)
        {
            this.AdaptForBinding();
        }

        public object DataContext
        {
            get { return BindingContext.DataContext; }
            set { BindingContext.DataContext = value; }
        }

        public IMvxViewModel ViewModel
        {
            get { return DataContext as IMvxViewModel; }
            set { DataContext = value; }
        }

        public MvxViewModelRequest? Request { get; set; }

        public IMvxBindingContext? BindingContext { get; set; }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            _ = ViewModel?.ViewCreated().AsTask();
        }

        public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);
            _ = ViewModel?.ViewAppearing().AsTask();
		}

		public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);
            _ = ViewModel?.ViewAppeared().AsTask();
		}

		public override void ViewWillDisappear(bool animated)
		{
			base.ViewWillDisappear(animated);
            _ = ViewModel?.ViewDisappearing().AsTask();
		}

		public override void ViewDidDisappear(bool animated)
		{
			base.ViewDidDisappear(animated);

			_ = ViewModel?.ViewDisappeared().AsTask();
		}

        public override void DidMoveToParentViewController(UIViewController? parent)
        {
            base.DidMoveToParentViewController(parent);

            if (parent == null)
            {
                _ = ViewModel?.ViewDestroy().AsTask();
            }
        }

        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject? sender)
        {
            base.PrepareForSegue(segue, sender);

            this.ViewModelRequestForSegue(segue, sender);
        }
    }

    public class MvxViewController<TViewModel>
        : MvxViewController, IMvxIosView<TViewModel>
        where TViewModel : class, IMvxViewModel
    {
        public MvxViewController()
        {
        }

        public MvxViewController(NSCoder coder)
            : base(coder)
        {
        }

        public MvxViewController(string? nibName, NSBundle? bundle)
            : base(nibName, bundle)
        {
        }

        protected MvxViewController(NSObjectFlag t)
            : base(t)
        {
        }

        protected internal MvxViewController(IntPtr handle)
            : base(handle)
        {
        }

        public new TViewModel ViewModel
        {
            get { return (TViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }

        public MvxFluentBindingDescriptionSet<IMvxIosView<TViewModel>, TViewModel> CreateBindingSet()
        {
            return this.CreateBindingSet<IMvxIosView<TViewModel>, TViewModel>();
        }
    }
}
