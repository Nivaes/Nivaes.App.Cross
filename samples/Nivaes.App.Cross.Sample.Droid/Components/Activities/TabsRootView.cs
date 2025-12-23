// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics.CodeAnalysis;
using AndroidX.ViewPager.Widget;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using Nivaes.App.Cross.Droid;
using Playground.Core.ViewModels;
using Resource = Nivaes.App.Cross.Sample.Droid.Resource;

namespace Playground.Droid.Activities
{
    [MvxActivityPresentation]
    [Activity(Theme = "@style/AppTheme", ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize)]
    [RequiresUnreferencedCode("MvxBindings require unreferenced code")]
    public class TabsRootView : MvxActivity<TabsRootViewModel>
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.TabsRootView);

            var viewPager = FindViewById<ViewPager>(Resource.Id.viewpager);
            if (viewPager.Adapter is not MvxCachingFragmentStatePagerAdapter)
                viewPager.Adapter = new MvxCachingFragmentStatePagerAdapter(SupportFragmentManager, new());

            if (savedInstanceState == null)
            {
                ViewModel.ShowInitialViewModelsCommand.Execute();
            }
        }
    }
}
