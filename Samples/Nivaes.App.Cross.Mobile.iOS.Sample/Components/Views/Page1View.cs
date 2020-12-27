// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross.Mobile.iOS.Sample
{
    using System;
    using CoreGraphics;
    using Foundation;
    using MvvmCross.Binding.BindingContext;
    using MvvmCross.Platforms.Ios.Binding;
    using MvvmCross.Platforms.Ios.Binding.Views;
    using MvvmCross.Platforms.Ios.Views;
    using MvvmCross.Platforms.Ios.Views.Expandable;
    using Nivaes.App.Cross.Presenters;
    using Nivaes.App.Cross.Sample;
    using UIKit;

    [MvxFromStoryboard("PagesRootView")]
    [MvxPagePresentation(WrapInNavigationController = false)]
    public partial class Page1View
        : MvxViewController<Page1ViewModel>
    {
        private UITableView mTableView;
        private TableSource mSource;

        public Page1View(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            Title = "Page 1";

            mTableView = new UITableView
            {
                TranslatesAutoresizingMaskIntoConstraints = false
            };

            mTableView.TableFooterView = new UIView();

            mTableView.RegisterClassForCellReuse(typeof(HeaderCell), HeaderCell.Identifier);
            mTableView.RegisterClassForCellReuse(typeof(ItemCell), ItemCell.Identifier);

            mSource = new TableSource(mTableView);
            mTableView.Source = mSource;

            Add(mTableView);

            mTableView.LeadingAnchor.ConstraintEqualTo(View.LeadingAnchor).Active = true;
            mTableView.TrailingAnchor.ConstraintEqualTo(View.TrailingAnchor).Active = true;
            mTableView.TopAnchor.ConstraintEqualTo(View.SafeAreaLayoutGuide.TopAnchor).Active = true;
            mTableView.BottomAnchor.ConstraintEqualTo(View.SafeAreaLayoutGuide.BottomAnchor).Active = true;

            var set = CreateBindingSet();
            set.Bind(mSource).To(vm => vm.Sections);
            set.Bind(mSource).For(v => v.HeaderTappedCommand).To(vm => vm.HeaderTappedCommand);
            set.Apply();
        }

        private class HeaderCell : MvxTableViewCell, IExpandableHeaderCell
        {
            public static NSString Identifier = new NSString(nameof(HeaderCell));
            private static UIImage mArrowImage;
            private UILabel mTitle;
            private UIImageView mArrow;
            private UISwitch mSwitch;

            public HeaderCell(IntPtr handle) : base(handle)
            {
                Initialize();
            }

            public HeaderCell()
            {
                Initialize();
            }

            private void Initialize()
            {
                if (mArrowImage == null)
                    mArrowImage = UIImage.FromBundle("ArrowDown");

                mTitle = new UILabel
                {
                    TranslatesAutoresizingMaskIntoConstraints = false
                };

                mArrow = new UIImageView(mArrowImage)
                {
                    TranslatesAutoresizingMaskIntoConstraints = false,
                    Hidden = true
                };

                mSwitch = new UISwitch
                {
                    TranslatesAutoresizingMaskIntoConstraints = false,
                    TintColor = UIColor.Blue,
                    Hidden = true
                };

                ContentView.Add(mTitle);
                ContentView.Add(mArrow);
                ContentView.Add(mSwitch);

                mTitle.LeadingAnchor.ConstraintEqualTo(ContentView.LeadingAnchor, 10).Active = true;
                mTitle.CenterYAnchor.ConstraintEqualTo(ContentView.CenterYAnchor).Active = true;

                mArrow.TrailingAnchor.ConstraintEqualTo(ContentView.TrailingAnchor, -10).Active = true;
                mArrow.CenterYAnchor.ConstraintEqualTo(ContentView.CenterYAnchor).Active = true;
                mArrow.WidthAnchor.ConstraintEqualTo(15).Active = true;
                mArrow.HeightAnchor.ConstraintEqualTo(mArrow.WidthAnchor).Active = true;

                mSwitch.TrailingAnchor.ConstraintEqualTo(ContentView.TrailingAnchor, -10).Active = true;
                mSwitch.CenterYAnchor.ConstraintEqualTo(ContentView.CenterYAnchor).Active = true;

                var set = this.CreateBindingSet<HeaderCell, Page1ViewModel.SectionViewModel>();
                set.Bind(mTitle).To(vm => vm.Title);
                set.Bind(mArrow).For(v => v.BindHidden()).To(vm => vm.ShowsControl);
                set.Bind(mSwitch).For(v => v.BindVisible()).To(vm => vm.ShowsControl);
                set.Bind(mSwitch).To(vm => vm.On);
                set.Apply();
            }

            public void OnCollapsed()
            {
                mArrow.Transform = CGAffineTransform.MakeIdentity();
            }

            public void OnExpanded()
            {
                mArrow.Transform = CGAffineTransform.MakeRotation(180 * ((float)Math.PI / 180.0f));
            }
        }

        private class ItemCell : MvxTableViewCell
        {
            public static NSString Identifier = new NSString(nameof(ItemCell));
            private UILabel _title;

            public ItemCell(IntPtr handle) : base(handle)
            {
                Initialize();
            }

            public ItemCell()
            {
                Initialize();
            }

            private void Initialize()
            {
                _title = new UILabel
                {
                    TranslatesAutoresizingMaskIntoConstraints = false
                };

                ContentView.Add(_title);

                _title.LeadingAnchor.ConstraintEqualTo(ContentView.LeadingAnchor, 10).Active = true;
                _title.TrailingAnchor.ConstraintEqualTo(ContentView.TrailingAnchor, -10).Active = true;
                _title.CenterYAnchor.ConstraintEqualTo(ContentView.CenterYAnchor).Active = true;

                var set = this.CreateBindingSet<ItemCell, Page1ViewModel.SectionItemViewModel>();
                set.Bind(_title).To(vm => vm.Title);
                set.Apply();
            }
        }

        private class TableSource : MvxExpandableTableViewSource<Page1ViewModel.SectionViewModel, Page1ViewModel.SectionItemViewModel>
        {
            public TableSource(UITableView tableView) : base(tableView)
            {
            }

            protected override UITableViewCell GetOrCreateCellFor(UITableView tableView, NSIndexPath indexPath, object item)
            {
                return TableView.DequeueReusableCell(ItemCell.Identifier);
            }

            protected override UITableViewCell GetOrCreateHeaderCellFor(UITableView tableView, nint section)
            {
                return tableView.DequeueReusableCell(HeaderCell.Identifier);
            }
        }
    }
}
