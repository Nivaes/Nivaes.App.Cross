// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;
using MvvmCross.Exceptions;
using MvvmCross.Logging;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.Presenters;
using MvvmCross.ViewModels;
using Nivaes.App.Cross;

namespace MvvmCross.Platforms.Android.Views
{
    public static class MvxFragmentExtensions
    {
        [return: DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
        public static Type FindAssociatedViewModelType(this IMvxFragmentView fragmentView, Type fragmentActivityParentType)
        {
            var viewModelType = fragmentView.FindAssociatedViewModelTypeOrNull();

            var type = fragmentView.GetType();

            if (viewModelType == null)
            {
                if (!type.HasBasePresentationAttribute())
                    throw new InvalidOperationException($"Your fragment of type {type.FullName} is not generic and it does not have {nameof(MvxFragmentPresentationAttribute)} attribute set!");

                var cacheableFragmentAttribute = type.GetBasePresentationAttribute();
                if (cacheableFragmentAttribute.ViewModelType == null)
                    throw new InvalidOperationException($"Your fragment of type {type.FullName} is not generic and it does not use {nameof(MvxFragmentPresentationAttribute)} with ViewModel Type constructor.");

                viewModelType = cacheableFragmentAttribute.ViewModelType;
            }

            return viewModelType;
        }

        public static ICrossViewModel LoadViewModel(this IMvxFragmentView fragmentView, ICrossBundle savedState, Type fragmentParentActivityType,
            CrossViewModelRequest request = null)
        {
            var viewModelType = fragmentView.FindAssociatedViewModelType(fragmentParentActivityType);
            if (viewModelType == typeof(CrossNullViewModel))
                return new CrossNullViewModel();

            if (viewModelType == null
                || viewModelType == typeof(ICrossViewModel))
            {
                MvxLogHost.Default?.Log(LogLevel.Trace,
                    "No ViewModel class specified for {FragmentViewType} in LoadViewModel",
                    fragmentView.GetType().Name);
            }

            if (request == null)
                request = CrossViewModelRequest.GetDefaultRequest(viewModelType);

            var viewModelCache = Mvx.IoCProvider.Resolve<ICrossChildViewModelCache>();
            if (viewModelCache.Exists(viewModelType))
            {
                var viewModelCached = viewModelCache.Get(viewModelType);
                viewModelCache.Remove(viewModelType);
                return viewModelCached;
            }

            var loaderService = Mvx.IoCProvider.Resolve<ICrossViewModelLoader>();
            var viewModel = loaderService.LoadViewModel(request, savedState);

            return viewModel;
        }

        [UnconditionalSuppressMessage("Trimming", "IL2072", Justification = "ViewModel types are preserved by the navigation infrastructure.")]
        public static void RunViewModelLifecycle(ICrossViewModel viewModel, ICrossBundle savedState,
            CrossViewModelRequest request)
        {
            try
            {
                if (request != null)
                {
                    var parameterValues = new CrossBundle(request.ParameterValues);
                    viewModel.CallBundleMethods("Init", parameterValues);
                }
                if (savedState != null)
                {
                    viewModel.CallBundleMethods("ReloadState", savedState);
                }
                viewModel.Start();
            }
            catch (Exception exception)
            {
                throw exception.MvxWrap("Problem running viewModel lifecycle of type {0}", viewModel.GetType().Name);
            }
        }

        public static string FragmentJavaName(this Type fragmentType)
        {
            return Java.Lang.Class.FromType(fragmentType).Name;
        }
    }
}
