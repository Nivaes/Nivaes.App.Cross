﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.Platforms.Uap.Views
{
    using System;
    using System.Collections.Generic;
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using Microsoft.UI.Xaml.Navigation;
    using MvvmCross.Platforms.Uap.Views.Suspension;
    using MvvmCross.Views;
    using Nivaes.App.Cross;
    using Nivaes.App.Cross.ViewModels;
    using Windows.UI.Core;

    public class MvxWindowsPage
        : Page
        , IMvxWindowsView
        , IDisposable
    {
        public MvxWindowsPage()
        {
            Loading += MvxWindowsPage_Loading;
            Loaded += MvxWindowsPage_Loaded;
            Unloaded += MvxWindowsPage_Unloaded;
        }

        private void MvxWindowsPage_Loading(FrameworkElement sender, object args)
        {
            _ = ViewModel?.ViewAppearing().AsTask();
        }

        private void MvxWindowsPage_Loaded(object sender, RoutedEventArgs e)
        {
            _ = ViewModel?.ViewAppeared().AsTask();
        }

        private void MvxWindowsPage_Unloaded(object sender, RoutedEventArgs e)
        {
            _ = ViewModel?.ViewDestroy().AsTask();
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            _ = ViewModel?.ViewDisappearing().AsTask();
            base.OnNavigatingFrom(e);
        }

        private IMvxViewModel? mViewModel;

        public IMvxWindowsFrame WrappedFrame => new MvxWrappedFrame(Frame);

        public IMvxViewModel? ViewModel
        {
            get
            {
                return mViewModel;
            }
            set
            {
                if (mViewModel == value)
                    return;

                mViewModel = value;
                DataContext = ViewModel;
                OnViewModelSet();
            }
        }

        protected virtual void OnViewModelSet()
        {
        }

        public virtual void ClearBackStack()
        {
            var backStack = base.Frame?.BackStack;

            while (backStack != null && backStack.Count > 0)
            {
                backStack.RemoveAt(0);
            }

            UpdateBackButtonVisibility();
        }

        protected virtual void UpdateBackButtonVisibility()
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
        }

        private string _reqData = string.Empty;

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            _ = ViewModel?.ViewCreated().AsTask();

            if (_reqData != string.Empty)
            {
                var viewModelLoader = Mvx.IoCProvider.Resolve<IMvxWindowsViewModelLoader>();
                ViewModel = await viewModelLoader.Load(e.Parameter.ToString(), LoadStateBundle(e)).ConfigureAwait(true);
                _ = ViewModel?.ViewCreated().AsTask();
            }
            _reqData = (string)e.Parameter;

            this.OnViewCreate(_reqData, () => LoadStateBundle(e));
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            _ = ViewModel?.ViewDisappeared().AsTask();
            base.OnNavigatedFrom(e);
            var bundle = this.CreateSaveStateBundle();
            SaveStateBundle(e, bundle);

            var translator = Mvx.IoCProvider.Resolve<IMvxWindowsViewModelRequestTranslator>();

            if (e.NavigationMode == NavigationMode.Back)
            {
                var key = translator.RequestTextGetKey(_reqData);
                this.OnViewDestroy(key);
            }
            else
            {
                var backstack = Frame.BackStack;
                if (backstack.Count > 0)
                {
                    var currentEntry = backstack[backstack.Count - 1];
                    var key = translator.RequestTextGetKey(currentEntry.Parameter.ToString());
                    if (key == 0)
                    {
                        var newParamter = translator.GetRequestTextWithKeyFor(ViewModel);
                        var entry = new PageStackEntry(currentEntry.SourcePageType, newParamter, currentEntry.NavigationTransitionInfo);
                        backstack.Remove(currentEntry);
                        backstack.Add(entry);
                    }
                }
            }
        }

        private string? mPageKey;

        private IMvxSuspensionManager? mSuspensionManager;

        protected IMvxSuspensionManager SuspensionManager
        {
            get
            {
                mSuspensionManager ??= Mvx.IoCProvider.Resolve<IMvxSuspensionManager>();
                return mSuspensionManager;
            }
        }

        protected virtual IMvxBundle? LoadStateBundle(NavigationEventArgs e)
        {
            // nothing loaded by default
            var frameState = SuspensionManager.SessionStateForFrame(WrappedFrame);
            mPageKey = $"Page-{Frame.BackStackDepth}";
            IMvxBundle? bundle = null;

            if (e.NavigationMode == NavigationMode.New)
            {
                // Clear existing state for forward navigation when adding a new page to the
                // navigation stack
                var nextPageKey = mPageKey;
                var nextPageIndex = Frame.BackStackDepth;
                while (frameState.Remove(nextPageKey))
                {
                    nextPageIndex++;
                    nextPageKey = "Page-" + nextPageIndex;
                }
            }
            else
            {
                var dictionary = (IDictionary<string, string>)frameState[mPageKey];
                bundle = new MvxBundle(dictionary);
            }

            return bundle;
        }

        protected virtual void SaveStateBundle(NavigationEventArgs navigationEventArgs, IMvxBundle? bundle)
        {
            var frameState = SuspensionManager.SessionStateForFrame(WrappedFrame);
            frameState[mPageKey!] = bundle?.Data;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~MvxWindowsPage()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Loading -= MvxWindowsPage_Loading;
                Loaded -= MvxWindowsPage_Loaded;
                Unloaded -= MvxWindowsPage_Unloaded;
            }
        }
    }

    public class MvxWindowsPage<TViewModel>
        : MvxWindowsPage
        , IMvxWindowsView<TViewModel> where TViewModel : class, IMvxViewModel
    {
        public new TViewModel ViewModel
        {
            get { return (TViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }
    }
}
