// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Android.Content;
using Android.OS;
using Android.Runtime;

namespace MvvmCross.Platforms.Android.Views.ViewPager
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AndroidX.Fragment.App;
    using Java.Lang;
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross.Presenters;
    using JavaObject = Java.Lang.Object;
    using JavaString = Java.Lang.String;

    [Register("con.nivaes.app.MvxCachingFragmentStatePagerAdapter")]
    public class MvxCachingFragmentStatePagerAdapter
        : MvxCachingFragmentPagerAdapter
    {
        private readonly Context? mContext;
        private readonly Type mActivityType;

        public List<MvxViewPagerFragmentInfo>? FragmentsInfo { get; }

        public override int Count => FragmentsInfo?.Count ?? 0;

        protected MvxCachingFragmentStatePagerAdapter(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
            mActivityType = Mvx.IoCProvider.Resolve<IMvxAndroidCurrentTopActivity>().Activity.GetType();
        }

        public MvxCachingFragmentStatePagerAdapter(Context context, FragmentManager fragmentManager,
            List<MvxViewPagerFragmentInfo> fragmentsInfo) : base(fragmentManager)
        {
            mContext = context;
            FragmentsInfo = fragmentsInfo;
            mActivityType = Mvx.IoCProvider.Resolve<IMvxAndroidCurrentTopActivity>().Activity.GetType();
        }

        public override Fragment GetItem(int position, Fragment.SavedState? fragmentSavedState = null)
        {
            var fragmentInfo = FragmentsInfo![position];
            var fragmentClass = Class.FromType(fragmentInfo.FragmentType);
            var fragment = FragmentFactory.Instantiate(
                fragmentClass.ClassLoader,
                fragmentClass.Name
            );

            if (!(fragment is IMvxFragmentView mvxFragment))
            {
                return fragment;
            }

            if (mvxFragment.GetType().IsFragmentCacheable(mActivityType) && fragmentSavedState != null)
            {
                return fragment;
            }

            mvxFragment.ViewModel = GetViewModel(fragmentInfo);

            fragment.Arguments = GetArguments(fragmentInfo);

            return fragment;
        }

        public override int GetItemPosition(JavaObject @object)
        {
            return PositionNone;
        }

        public override ICharSequence GetPageTitleFormatted(int position)
        {
            return new JavaString(FragmentsInfo.ElementAt(position).Title);
        }

        protected override string GetTag(int position)
        {
            return FragmentsInfo.ElementAt(position).Tag;
        }

        private static IMvxViewModel GetViewModel(MvxViewPagerFragmentInfo fragmentInfo)
        {
            if (fragmentInfo.Request is MvxViewModelInstanceRequest instanceRequest)
            {
                return instanceRequest.ViewModelInstance;
            }

            var viewModelLoader = Mvx.IoCProvider.Resolve<IMvxViewModelLoader>();

            return viewModelLoader.LoadViewModel(fragmentInfo.Request, null);
        }

        private static Bundle GetArguments(MvxViewPagerFragmentInfo fragmentInfo)
        {
            var navigationSerializer = Mvx.IoCProvider.Resolve<IMvxNavigationSerializer>();

            var serializedRequest = navigationSerializer.Serializer.SerializeObject(fragmentInfo.Request);

            var bundle = new Bundle();

            bundle.PutString(MvxAndroidViewPresenter.ViewModelRequestBundleKey, serializedRequest);

            return bundle;
        }
    }
}
