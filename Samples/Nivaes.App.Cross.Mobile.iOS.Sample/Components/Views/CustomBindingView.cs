// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross.Mobile.iOS.Sample
{
    using System;
    using System.Drawing;
    using MvvmCross.Platforms.Ios.Views;
    using Nivaes.App.Cross.Presenters;
    using Nivaes.App.Cross.Sample;
    using ObjCRuntime;
    using UIKit;

    [MvxFromStoryboard("CustomBindingView")]
    [MvxChildPresentation]
    public partial class CustomBindingView
        : MvxViewController<CustomBindingViewModel>
    {
        private UIDatePicker _datePicker;

        public CustomBindingView(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            View = new UIView
            {
                BackgroundColor = UIColor.White
            };
            base.ViewDidLoad();

            _datePicker = new UIDatePicker();
            View.AddSubview(_datePicker);

            // ios7 layout
            if (RespondsToSelector(new Selector("edgesForExtendedLayout")))
            {
                EdgesForExtendedLayout = UIRectEdge.None;
            }

            var binaryEdit = new BinaryEdit(new RectangleF(10, 70, 300, 120));
            Add(binaryEdit);
            var textField = new UITextField(new RectangleF(10, 190, 300, 40));
            Add(textField);

            var set = CreateBindingSet();
            set.Bind(binaryEdit).For("MyCount").To(vm => vm.Counter);
            set.Bind(textField).To(vm => vm.Counter);
            set.Bind(_datePicker).For(v => v.Date).To(vm => vm.Date);
            set.Apply();

            var tap = new UITapGestureRecognizer(() => textField.ResignFirstResponder());
            View.AddGestureRecognizer(tap);
        }
    }
}

