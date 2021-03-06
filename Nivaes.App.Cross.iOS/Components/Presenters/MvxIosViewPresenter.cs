﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross.Presenters
{

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using CoreGraphics;
    using MvvmCross.Exceptions;
    using MvvmCross.Platforms.Ios.Presenters.Attributes;
    using MvvmCross.Platforms.Ios.Views;
    using Nivaes.App.Cross.Logging;
    using Nivaes.App.Cross.ViewModels;
    using UIKit;

    public class MvxIosViewPresenter
        : MvxAttributeViewPresenter, IMvxIosViewPresenter
    {
        protected readonly IUIApplicationDelegate mApplicationDelegate;
        protected readonly UIWindow mWindow;

        public UINavigationController MasterNavigationController { get; protected set; }

        public List<UIViewController> ModalViewControllers { get; protected set; } = new List<UIViewController>();

        public IMvxTabBarViewController TabBarViewController { get; protected set; }

        public IMvxPageViewController PageViewController { get; protected set; }

        public IMvxSplitViewController SplitViewController { get; protected set; }

        public override ValueTask<MvxBasePresentationAttribute?> CreatePresentationAttribute(Type viewModelType, Type viewType)
        {
            if (MasterNavigationController == null &&
               (TabBarViewController == null ||
               !TabBarViewController.CanShowChildView()))
            {
                //MvxLog.Instance?.Trace($"PresentationAttribute nor MasterNavigationController found for {viewType.Name}. " +
                //    $"Assuming Root presentation");
                return new ValueTask<MvxBasePresentationAttribute?>(new MvxRootPresentationAttribute() { WrapInNavigationController = true, ViewType = viewType, ViewModelType = viewModelType });
            }

            //MvxLog.Instance?.Trace($"PresentationAttribute not found for {viewType.Name}. " +
            //    $"Assuming animated Child presentation");
            return new ValueTask<MvxBasePresentationAttribute?>(new MvxChildPresentationAttribute() { ViewType = viewType, ViewModelType = viewModelType });
        }

        public override ValueTask<MvxBasePresentationAttribute?> GetOverridePresentationAttribute(MvxViewModelRequest request, Type viewType)
        {
            if (viewType?.GetInterface(nameof(IMvxOverridePresentationAttribute)) != null)
            {
                var viewInstance = this.CreateViewControllerFor(viewType, null) as UIViewController;
                using (viewInstance)
                {
                    var presentationAttribute = (viewInstance as IMvxOverridePresentationAttribute)?.PresentationAttribute(request);

                    if (presentationAttribute == null)
                    {
                        //MvxLog.Instance?.Warn("Override PresentationAttribute null. Falling back to existing attribute.");
                    }
                    else
                    {
                        if (presentationAttribute.ViewType == null)
                            presentationAttribute.ViewType = viewType;

                        if (presentationAttribute.ViewModelType == null)
                            presentationAttribute.ViewModelType = request.ViewModelType;

                        return new ValueTask<MvxBasePresentationAttribute?>(presentationAttribute);
                    }
                }
            }

            return new ValueTask<MvxBasePresentationAttribute?>((MvxBasePresentationAttribute?)null);
        }

        public MvxIosViewPresenter(IUIApplicationDelegate applicationDelegate, UIWindow window)
        {
            mApplicationDelegate = applicationDelegate;
            mWindow = window;
        }

        public override void RegisterAttributeTypes()
        {
            AttributeTypesToActionsDictionary.Register<MvxRootPresentationAttribute>(
                (viewType, attribute, request) =>
                    {
                        var viewController = (UIViewController)this.CreateViewControllerFor(request);
                        return ShowRootViewController(viewController, attribute, request);
                    },
                    CloseRootViewController);

            AttributeTypesToActionsDictionary.Register<MvxChildPresentationAttribute>(
                (viewType, attribute, request) =>
                    {
                        var viewController = (UIViewController)this.CreateViewControllerFor(request);
                        return ShowChildViewController(viewController, attribute, request);
                    },
                    CloseChildViewController);

            AttributeTypesToActionsDictionary.Register<MvxTabPresentationAttribute>(
                (viewType, attribute, request) =>
                    {
                        var viewController = (UIViewController)this.CreateViewControllerFor(request);
                        return ShowTabViewController(viewController, attribute, request);
                    },
                    CloseTabViewController);

            AttributeTypesToActionsDictionary.Register<MvxPagePresentationAttribute>(
                (viewType, attribute, request) =>
                    {
                        var viewController = (UIViewController)this.CreateViewControllerFor(request);
                        return ShowPageViewController(viewController, attribute, request);
                    },
                    ClosePageViewController);

            AttributeTypesToActionsDictionary.Register<MvxModalPresentationAttribute>(
                (viewType, attribute, request) =>
                    {
                        var viewController = (UIViewController)this.CreateViewControllerFor(request);
                        return ShowModalViewController(viewController, attribute, request);
                    },
                    CloseModalViewController);

            AttributeTypesToActionsDictionary.Register<MvxSplitViewPresentationAttribute>(
                (viewType, attribute, request) =>
                    {
                        var viewController = (UIViewController)this.CreateViewControllerFor(request);
                        var splitAttribute = attribute;
                        switch (splitAttribute.Position)
                        {
                            case MasterDetailPosition.Master:
                                return ShowMasterSplitViewController(viewController, splitAttribute, request);

                            case MasterDetailPosition.Detail:
                                return ShowDetailSplitViewController(viewController, splitAttribute, request);
                        }
                        return new ValueTask<bool>(true);
                    },
                (viewModel, attribute) =>
                    {
                        var splitAttribute = attribute;
                        switch (splitAttribute.Position)
                        {
                            case MasterDetailPosition.Master:
                                return CloseMasterSplitViewController(viewModel, splitAttribute);

                            case MasterDetailPosition.Detail:
                            default:
                                return CloseDetailSplitViewController(viewModel, splitAttribute);
                        }
                    });
        }

        protected virtual async ValueTask<bool> ShowRootViewController(
            UIViewController viewController,
            MvxRootPresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            // check if viewController is a TabBarController
            if (viewController is IMvxTabBarViewController tabBarController)
            {
                TabBarViewController = tabBarController;

                // set root
                SetupWindowRootNavigation(viewController, attribute);

                if (!await CloseModalViewControllers()) return false;
                if (!await CloseSplitViewController()) return false;

                return true;
            }

            if (viewController is IMvxPageViewController pageViewController)
            {
                PageViewController = pageViewController;

                // set root
                SetupWindowRootNavigation(viewController, attribute);

                if (!await CloseModalViewControllers()) return false;
                if (!await CloseSplitViewController()) return false;

                return true;
            }

            // check if viewController is a SplitViewController
            if (viewController is IMvxSplitViewController splitController)
            {
                SplitViewController = splitController;

                // set root
                SetupWindowRootNavigation(viewController, attribute);

                if (!await CloseModalViewControllers()) return false;
                if (!await CloseTabBarViewController()) return false;

                return true;
            }

            // set root initiating stack navigation or just a plain controller
            SetupWindowRootNavigation(viewController, attribute);

            if (!await CloseModalViewControllers()) return false;
            if (!await CloseTabBarViewController()) return false;
            if (!await CloseSplitViewController()) return false;
            return true;
        }

        public override ValueTask<bool> ChangePresentation(MvxPresentationHint hint)
        {
            if (hint is MvxPagePresentationHint pagePresentationHint)
            {
                if (TabBarViewController is UITabBarController tabsController
                    && tabsController.ViewControllers != null)
                {
                    foreach (var vc in tabsController.ViewControllers)
                    {
                        IMvxIosView tabView;

                        if (vc is UINavigationController)
                        {
                            var root = ((UINavigationController)vc).ViewControllers.FirstOrDefault();
                            tabView = root.GetIMvxIosView();
                        }
                        else
                        {
                            tabView = vc.GetIMvxIosView();
                        }

                        var viewModelType = tabView.GetViewModelType();

                        if (viewModelType != null && viewModelType == pagePresentationHint.ViewModel)
                        {
                            tabsController.SelectedViewController = vc;
                            return new ValueTask<bool>(true);
                        }
                    }
                }
            }

            return base.ChangePresentation(hint);
        }

        protected void SetupWindowRootNavigation(UIViewController viewController, MvxRootPresentationAttribute attribute)
        {
            if (attribute.WrapInNavigationController)
            {
                MasterNavigationController = CreateNavigationController(viewController);

                SetWindowRootViewController(MasterNavigationController, attribute);
            }
            else
            {
                SetWindowRootViewController(viewController, attribute);

                CloseMasterNavigationController();
            }
        }

        protected virtual ValueTask<bool> ShowChildViewController(
            UIViewController viewController,
            MvxChildPresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            if (viewController is IMvxSplitViewController)
                throw new MvxException("A SplitViewController cannot be presented as a child. Consider using Root instead");

            if (ModalViewControllers.Any())
            {
                if (ModalViewControllers.LastOrDefault() is UINavigationController modalNavController)
                {
                    PushViewControllerIntoStack(modalNavController, viewController, attribute);

                    return new ValueTask<bool>(true);
                }
                else
                {
                    throw new MvxException($"Trying to show View type: {viewController.GetType().Name} as child, but there is currently a plain modal view presented!");
                }
            }

            if (TabBarViewController != null && TabBarViewController.ShowChildView(viewController))
            {
                return new ValueTask<bool>(true);
            }

            if (MasterNavigationController != null)
            {
                PushViewControllerIntoStack(MasterNavigationController, viewController, attribute);
                return new ValueTask<bool>(true);
            }

            throw new MvxException($"Trying to show View type: {viewController.GetType().Name} as child, but there is no current stack!");
        }

        protected virtual ValueTask<bool> ShowTabViewController(
            UIViewController viewController,
            MvxTabPresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            if (TabBarViewController == null)
                throw new MvxException("Trying to show a tab without a TabBarViewController, this is not possible!");

            if (viewController is IMvxTabBarItemViewController tabBarItem)
            {
                attribute.TabName = tabBarItem.TabName;
                attribute.TabIconName = tabBarItem.TabIconName;
                attribute.TabSelectedIconName = tabBarItem.TabSelectedIconName;
            }

            if (attribute.WrapInNavigationController)
                viewController = CreateNavigationController(viewController);

            TabBarViewController.ShowTabView(
                viewController,
                attribute);
            return new ValueTask<bool>(true);
        }

        protected virtual ValueTask<bool> ShowPageViewController(
            UIViewController viewController,
            MvxPagePresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            if (PageViewController == null)
                throw new MvxException("Trying to show a page without a PageViewController, this is not possible!");

            /*if (viewController is IMvxTabBarItemViewController tabBarItem)
            {
                attribute.TabName = tabBarItem.TabName;
                attribute.TabIconName = tabBarItem.TabIconName;
                attribute.TabSelectedIconName = tabBarItem.TabSelectedIconName;
            }*/

            if (attribute.WrapInNavigationController)
                viewController = CreateNavigationController(viewController);

            PageViewController.AddPage(
                viewController,
                attribute);
            return new ValueTask<bool>(true);
        }

        protected virtual ValueTask<bool> ShowModalViewController(
            UIViewController viewController,
            MvxModalPresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            // Content size should be set to a target view controller, not the navigation one
            if (attribute.PreferredContentSize != default(CGSize))
            {
                viewController.PreferredContentSize = attribute.PreferredContentSize;
            }

            // setup modal based on attribute
            if (attribute.WrapInNavigationController)
            {
                viewController = CreateNavigationController(viewController);
            }

            viewController.ModalPresentationStyle = attribute.ModalPresentationStyle;
            viewController.ModalTransitionStyle = attribute.ModalTransitionStyle;

            // Check if there is a modal already presented first. Otherwise use the window root
            var modalHost = ModalViewControllers.LastOrDefault() ?? mWindow.RootViewController;

            modalHost.PresentViewController(
                viewController,
                attribute.Animated,
                null);

            ModalViewControllers.Add(viewController);
            return new ValueTask<bool>(true);
        }

        protected virtual ValueTask<bool> ShowMasterSplitViewController(
            UIViewController viewController,
            MvxSplitViewPresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            if (SplitViewController == null)
                throw new MvxException("Trying to show a master page without a SplitViewController, this is not possible!");

            SplitViewController.ShowMasterView(viewController, attribute);
            return new ValueTask<bool>(true);
        }

        protected virtual ValueTask<bool> ShowDetailSplitViewController(
            UIViewController viewController,
            MvxSplitViewPresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            if (SplitViewController == null)
                throw new MvxException("Trying to show a detail page without a SplitViewController, this is not possible!");

            SplitViewController.ShowDetailView(viewController, attribute);
            return new ValueTask<bool>(true);
        }

        protected virtual ValueTask<bool> CloseRootViewController(IMvxViewModel viewModel, MvxRootPresentationAttribute attribute)
        {
            //MvxLog.Instance?.Warn($"Ignored attempt to close the window root (ViewModel type: {viewModel.GetType().Name}");

            return new ValueTask<bool>(false);
        }

        protected virtual ValueTask<bool> CloseChildViewController(IMvxViewModel viewModel, MvxChildPresentationAttribute attribute)
        {
            // if there are modals presented
            if (ModalViewControllers.Any())
            {
                foreach (var modalNav in ModalViewControllers.Where(v => v is UINavigationController))
                {
                    if (TryCloseViewControllerInsideStack((UINavigationController)modalNav, viewModel, attribute))
                        return new ValueTask<bool>(true);
                }
            }

            //if the current root is a TabBarViewController, delegate close responsibility to it
            if (TabBarViewController != null && TabBarViewController.CloseChildViewModel(viewModel))
                return new ValueTask<bool>(true);

            if (SplitViewController != null && SplitViewController.CloseChildViewModel(viewModel, attribute))
                return new ValueTask<bool>(true);

            // if the current root is a NavigationController, close it in the stack
            if (MasterNavigationController != null && TryCloseViewControllerInsideStack(MasterNavigationController, viewModel, attribute))
                return new ValueTask<bool>(true);

            return new ValueTask<bool>(false);
        }

        protected virtual ValueTask<bool> CloseTabViewController(IMvxViewModel viewModel, MvxTabPresentationAttribute attribute)
        {
            if (TabBarViewController != null && TabBarViewController.CloseTabViewModel(viewModel))
                return new ValueTask<bool>(true);

            return new ValueTask<bool>(false);
        }

        protected virtual ValueTask<bool> ClosePageViewController(IMvxViewModel viewModel, MvxPagePresentationAttribute attribute)
        {
            if (PageViewController != null && PageViewController.RemovePage(viewModel))
                return new ValueTask<bool>(true);

            return new ValueTask<bool>(false);
        }

        protected virtual ValueTask<bool> CloseMasterSplitViewController(IMvxViewModel viewModel, MvxSplitViewPresentationAttribute attribute)
        {
            if (SplitViewController != null && SplitViewController.CloseChildViewModel(viewModel, attribute))
                return new ValueTask<bool>(true);

            return new ValueTask<bool>(true);
        }

        protected virtual ValueTask<bool> CloseDetailSplitViewController(IMvxViewModel viewModel, MvxSplitViewPresentationAttribute attribute)
        {
            if (SplitViewController != null && SplitViewController.CloseChildViewModel(viewModel, attribute))
                return new ValueTask<bool>(true);

            return new ValueTask<bool>(false);
        }

        protected virtual ValueTask<bool> CloseModalViewController(IMvxViewModel toClose, MvxModalPresentationAttribute attribute)
        {
            if (ModalViewControllers == null || !ModalViewControllers.Any())
                return new ValueTask<bool>(false);

            // check for plain modals
            var modalToClose = ModalViewControllers.FirstOrDefault(v => v is IMvxIosView && v.GetIMvxIosView().ViewModel == toClose);
            if (modalToClose != null)
            {
                return CloseModalViewController(modalToClose, attribute);
            }

            // check for modal navigation stacks
            UIViewController controllerToClose = null;
            foreach (var vc in ModalViewControllers.Where(v => v is UINavigationController))
            {
                var root = ((UINavigationController)vc).ViewControllers.FirstOrDefault();
                if (root != null && root.GetIMvxIosView().ViewModel == toClose)
                {
                    controllerToClose = vc;
                    break;
                }
            }
            if (controllerToClose != null)
            {
                return CloseModalViewController(controllerToClose, attribute);
            }

            return new ValueTask<bool>(false);
        }

        protected virtual bool TryCloseViewControllerInsideStack(UINavigationController navController, IMvxViewModel toClose, MvxChildPresentationAttribute attribute)
        {
            // check for top view controller
            var topView = navController.TopViewController.GetIMvxIosView();
            if (topView != null && topView.ViewModel == toClose)
            {
                navController.PopViewController(attribute.Animated);
                return true;
            }

            // loop through stack
            var controllers = navController.ViewControllers.ToList();
            var controllerToClose = controllers.FirstOrDefault(vc => vc.GetIMvxIosView().ViewModel == toClose);
            if (controllerToClose != null)
            {
                controllers.Remove(controllerToClose);
                navController.ViewControllers = controllers.ToArray();

                return true;
            }

            return false;
        }

        protected virtual MvxNavigationController CreateNavigationController(UIViewController viewController)
        {
            return new MvxNavigationController(viewController);
        }

        protected virtual void PushViewControllerIntoStack(UINavigationController navigationController, UIViewController viewController, MvxChildPresentationAttribute attribute)
        {
            navigationController.PushViewController(viewController, attribute.Animated);

            if (viewController is IMvxTabBarViewController tabBarController)
                TabBarViewController = tabBarController;
        }

        protected virtual void CloseMasterNavigationController()
        {
            if (MasterNavigationController == null)
                return;

            if (MasterNavigationController.ViewControllers != null)
            {
                foreach (var item in MasterNavigationController.ViewControllers)
                    item.DidMoveToParentViewController(null);
            }
            MasterNavigationController = null;
        }

        public virtual async ValueTask<bool> CloseModalViewController(UIViewController viewController, MvxModalPresentationAttribute attribute)
        {
            if (viewController == null)
                return true;

            if (viewController is UINavigationController modalNavController)
            {
                foreach (var item in modalNavController.ViewControllers)
                    item.DidMoveToParentViewController(null);
            }

            await viewController.DismissViewControllerAsync(attribute.Animated);
            ModalViewControllers.Remove(viewController);
            return true;
        }

        public virtual async ValueTask<bool> CloseModalViewControllers()
        {
            while (ModalViewControllers.Any())
            {
                if (!(await CloseModalViewController(ModalViewControllers.LastOrDefault(), new MvxModalPresentationAttribute()))) return false;
            }
            return true;
        }

        public virtual Task<bool> CloseTabBarViewController()
        {
            if (TabBarViewController == null)
                return Task.FromResult(true);

            if (TabBarViewController is UITabBarController tabsController
                && tabsController.ViewControllers != null)
            {
                foreach (var item in tabsController.ViewControllers)
                    item.DidMoveToParentViewController(null);
            }
            TabBarViewController = null;
            return Task.FromResult(true);
        }

        protected virtual Task<bool> CloseSplitViewController()
        {
            if (SplitViewController == null)
                return Task.FromResult(true);

            if (SplitViewController is UISplitViewController splitController
               && splitController.ViewControllers != null)
            {
                foreach (var item in splitController.ViewControllers)
                    item.DidMoveToParentViewController(null);
            }
            SplitViewController = null;
            return Task.FromResult(true);
        }

        protected void RemoveWindowSubviews()
        {
            foreach (var v in mWindow.Subviews)
                v.RemoveFromSuperview();
        }

        public virtual ValueTask<bool> ShowModalViewController(UIViewController viewController, bool animated)
        {
            return ShowModalViewController(viewController, new MvxModalPresentationAttribute { Animated = animated }, null);
        }

        protected virtual void SetWindowRootViewController(UIViewController controller, MvxRootPresentationAttribute attribute = null)
        {
            RemoveWindowSubviews();

            if (attribute == null || attribute.AnimationOptions == UIViewAnimationOptions.TransitionNone)
            {
                mWindow.RootViewController = controller;
                return;
            }

            UIView.Transition(
                mWindow, attribute.AnimationDuration, attribute.AnimationOptions,
                () => mWindow.RootViewController = controller, null
            );
        }
    }
}
