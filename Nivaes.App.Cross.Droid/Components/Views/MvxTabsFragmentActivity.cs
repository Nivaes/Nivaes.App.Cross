// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.Fragment.App;

namespace MvvmCross.Platforms.Android.Views
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Java.Lang;
    using MvvmCross.Base;
    using MvvmCross.Platforms.Android.Binding.BindingContext;
    using MvvmCross.ViewModels;
    using Object = Java.Lang.Object;

    [Register("mvvmcross.platforms.android.views.MvxTabsFragmentActivity")]
    public abstract class MvxTabsFragmentActivity
        : MvxActivity, TabHost.IOnTabChangeListener
    {
        private const string SavedTabIndexStateKey = "__savedTabIndex";
        private readonly Dictionary<string, TabInfo> mLookup = new Dictionary<string, TabInfo>();
        private readonly int mLayoutId;
        private TabHost? mTabHost;
        private TabInfo? mCurrentTab;
        private readonly int mTabContentId;

        protected MvxTabsFragmentActivity(int layoutId, int tabContentId)
        {
            mLayoutId = layoutId;
            mTabContentId = tabContentId;
        }

        protected class TabInfo
        {
            public string Tag { get; }
            public Type FragmentType { get;  }
            public Bundle Bundle { get; }
            public IMvxViewModel ViewModel { get; }

            public Fragment? CachedFragment { get; set; }

            public TabInfo(string tag, Type fragmentType, Bundle bundle, IMvxViewModel viewModel)
            {
                Tag = tag;
                FragmentType = fragmentType;
                Bundle = bundle;
                ViewModel = viewModel;
            }
        }

        private class TabFactory
            : Object, TabHost.ITabContentFactory
        {
            private readonly Context _context;

            public TabFactory(Context context)
            {
                _context = context;
            }

            public View CreateTabContent(string tag)
            {
                var v = new View(_context);
                v.SetMinimumWidth(0);
                v.SetMinimumHeight(0);
                return v;
            }
        }

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(mLayoutId);

            _view = Window.DecorView.RootView;

            InitializeTabHost(savedInstanceState);

            if (savedInstanceState != null)
            {
                mTabHost.SetCurrentTabByTag(savedInstanceState.GetString(SavedTabIndexStateKey));
            }
        }

        public override void SetContentView(int layoutResId)
        {
           var view = this.BindingInflate(layoutResId, null);

            SetContentView(view);
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            outState.PutString(SavedTabIndexStateKey, mTabHost.CurrentTabTag);
            base.OnSaveInstanceState(outState);
        }

        private void InitializeTabHost(Bundle args)
        {
            mTabHost = (TabHost)FindViewById(global::Android.Resource.Id.TabHost);
            mTabHost.Setup();

            AddTabs(args);

            if (mLookup.Any())
                OnTabChanged(mLookup.First().Key);

            mTabHost.SetOnTabChangedListener(this);
        }

        protected abstract void AddTabs(Bundle args);

        protected void AddTab<TFragment>(string tagAndSpecName, string tabName, Bundle args,
                                         IMvxViewModel viewModel)
        {
            var tabSpec = mTabHost.NewTabSpec(tagAndSpecName).SetIndicator(tabName);
            AddTab<TFragment>(args, viewModel, tabSpec);
        }

        protected void AddTab<TFragment>(Bundle args, IMvxViewModel viewModel, TabHost.TabSpec tabSpec)
        {
            var tabInfo = new TabInfo(tabSpec.Tag, typeof(TFragment), args, viewModel);
            AddTab(this, mTabHost, tabSpec, tabInfo);
            mLookup.Add(tabInfo.Tag, tabInfo);
        }

        private static void AddTab(MvxTabsFragmentActivity activity,
                                   TabHost tabHost,
                                   TabHost.TabSpec tabSpec,
                                   TabInfo tabInfo)
        {
            // Attach a Tab view factory to the spec
            tabSpec.SetContent(new TabFactory(activity));
            string tag = tabSpec.Tag;

            // Check to see if we already have a CachedFragment for this tab, probably
            // from a previously saved state.  If so, deactivate it, because our
            // initial state is that a tab isn't shown.
            tabInfo.CachedFragment = activity.SupportFragmentManager.FindFragmentByTag(tag);
            if (tabInfo.CachedFragment != null && !tabInfo.CachedFragment.IsDetached)
            {
                var ft = activity.SupportFragmentManager.BeginTransaction();
                ft.Detach(tabInfo.CachedFragment);
                ft.Commit();
                activity.SupportFragmentManager.ExecutePendingTransactions();
            }

            tabHost.AddTab(tabSpec);
        }

        public virtual void OnTabChanged(string tag)
        {
            var newTab = mLookup[tag];
            if (mCurrentTab != newTab)
            {
                var ft = SupportFragmentManager.BeginTransaction();
                OnTabFragmentChanging(tag, ft);
                if (mCurrentTab?.CachedFragment != null)
                {
                    ft.Detach(mCurrentTab.CachedFragment);
                }
                if (newTab != null)
                {
                    if (newTab.CachedFragment == null)
                    {
                        var fragmentClass = Class.FromType(newTab.FragmentType);
                        newTab.CachedFragment = SupportFragmentManager.FragmentFactory.Instantiate(
                            fragmentClass.ClassLoader,
                            fragmentClass.Name
                        );
                        FixupDataContext(newTab);
                        ft.Add(mTabContentId, newTab.CachedFragment, newTab.Tag);
                    }
                    else
                    {
                        FixupDataContext(newTab);
                        ft.Attach(newTab.CachedFragment);
                    }
                }

                mCurrentTab = newTab;
                ft.Commit();
                SupportFragmentManager.ExecutePendingTransactions();
            }
        }

        protected virtual void FixupDataContext(TabInfo newTab)
        {
            var consumer = newTab.CachedFragment as IMvxDataConsumer;
            if (consumer == null)
                return;

            if (consumer.DataContext != newTab.ViewModel)
                consumer.DataContext = newTab.ViewModel;
        }

        protected virtual string FragmentJavaName(Type fragmentType)
        {
            return Class.FromType(fragmentType).Name;
        }

        public virtual void OnTabFragmentChanging(string tag, FragmentTransaction transaction)
        {
        }
    }
}
