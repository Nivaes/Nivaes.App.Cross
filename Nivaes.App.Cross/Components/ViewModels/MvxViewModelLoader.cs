// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using MvvmCross.Exceptions;
using MvvmCross.Navigation.EventArguments;

namespace MvvmCross.ViewModels
{
    public class MvxViewModelLoader
        : IMvxViewModelLoader
    {
        protected IMvxViewModelLocatorCollection LocatorCollection { get; private set; }

        public MvxViewModelLoader(IMvxViewModelLocatorCollection locatorCollection)
        {
            LocatorCollection = locatorCollection;
        }

        // Reload should be used to re-run cached ViewModels lifecycle if required.
        public IMvxViewModel ReloadViewModel(IMvxViewModel viewModel, MvxViewModelRequest request, IMvxBundle savedState, IMvxNavigateEventArgs navigationArgs)
        {
            var viewModelLocator = FindViewModelLocator(request);

            var parameterValues = new MvxBundle(request.ParameterValues);
            try
            {
                viewModel = viewModelLocator.Reload(viewModel, parameterValues, savedState, navigationArgs);
            }
            catch (Exception ex)
            {
                throw new MvxException($"Failed to reload a previously created created ViewModel for type {request.ViewModelType} from locator {viewModelLocator.GetType().Name} - check InnerException for more information", ex);
            }

            return viewModel;
        }

        public IMvxViewModel ReloadViewModel<TParameter>(IMvxViewModel<TParameter> viewModel, TParameter param, MvxViewModelRequest request, IMvxBundle savedState, IMvxNavigateEventArgs navigationArgs)
        {
            var viewModelLocator = FindViewModelLocator(request);

            var parameterValues = new MvxBundle(request.ParameterValues);
            try
            {
                viewModel = viewModelLocator.Reload(viewModel, param, parameterValues, savedState, navigationArgs);
            }
            catch (Exception ex)
            {
                throw new MvxException($"Failed to reload a previously created created ViewModel for type {request.ViewModelType} from locator {viewModelLocator.GetType().Name} - check InnerException for more information", ex);
            }

            return viewModel;
        }

        public IMvxViewModel LoadViewModel(MvxViewModelRequest request, IMvxBundle savedState, IMvxNavigateEventArgs navigationArgs)
        {
            if (request.ViewModelType == typeof(MvxNullViewModel))
            {
                return new MvxNullViewModel();
            }

            var viewModelLocator = FindViewModelLocator(request);

            IMvxViewModel viewModel = null;
            var parameterValues = new MvxBundle(request.ParameterValues);
            try
            {
                viewModel = viewModelLocator.Load(request.ViewModelType, parameterValues, savedState, navigationArgs);
            }
            catch (Exception ex)
            {
                throw new MvxException($"Failed to construct and initialize ViewModel for type {request.ViewModelType} from locator {viewModelLocator.GetType().Name} - check InnerException for more information", ex);
            }
            return viewModel;
        }

        public IMvxViewModel LoadViewModel<TParameter>(MvxViewModelRequest request, TParameter param, IMvxBundle savedState, IMvxNavigateEventArgs navigationArgs)
        {
            if (request.ViewModelType == typeof(MvxNullViewModel))
            {
                return new MvxNullViewModel();
            }

            var viewModelLocator = FindViewModelLocator(request);

            IMvxViewModel<TParameter> viewModel = null;
            var parameterValues = new MvxBundle(request.ParameterValues);
            try
            {
                viewModel = viewModelLocator.Load(request.ViewModelType, param, parameterValues, savedState, navigationArgs);
            }
            catch (Exception ex)
            {
                throw new MvxException($"Failed to construct and initialize ViewModel for type {request.ViewModelType} from locator {viewModelLocator.GetType().Name} - check InnerException for more information", ex);
            }
            return viewModel;
        }

        private IMvxViewModelLocator FindViewModelLocator(MvxViewModelRequest request)
        {
            var viewModelLocator = LocatorCollection.FindViewModelLocator(request);

            if (viewModelLocator == null)
            {
                throw new MvxException($"Sorry - somehow there's no viewmodel locator registered for {request.ViewModelType}");
            }

            return viewModelLocator;
        }
    }
}
