// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross.ViewModels
{
    using System;
    using System.Threading.Tasks;
    using MvvmCross.Exceptions;
    using Nivaes.App.Cross.Navigation;

    public class MvxViewModelLoader
        : IMvxViewModelLoader
    {
        protected IMvxViewModelLocatorCollection LocatorCollection { get; }

        public MvxViewModelLoader(IMvxViewModelLocatorCollection locatorCollection)
        {
            LocatorCollection = locatorCollection;
        }

        // Reload should be used to re-run cached ViewModels lifecycle if required.
        public ValueTask<IMvxViewModel> ReloadViewModel(IMvxViewModel viewModel, MvxViewModelRequest request, IMvxBundle? savedState, IMvxNavigateEventArgs? navigationArgs)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var viewModelLocator = FindViewModelLocator(request);

            var parameterValues = new MvxBundle(request.ParameterValues);
            try
            {
                return viewModelLocator.Reload(viewModel, parameterValues, savedState, navigationArgs);
            }
            catch (Exception ex)
            {
                throw new MvxException($"Failed to reload a previously created created ViewModel for type {request.ViewModelType} from locator {viewModelLocator.GetType().Name} - check InnerException for more information", ex);
            }
        }

        public async ValueTask<IMvxViewModel> ReloadViewModel<TParameter>(IMvxViewModel<TParameter> viewModel, TParameter? param, MvxViewModelRequest request, IMvxBundle? savedState, IMvxNavigateEventArgs? navigationArgs)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var viewModelLocator = FindViewModelLocator(request);

            var parameterValues = new MvxBundle(request.ParameterValues);
            try
            {
                var viewModelReload = await viewModelLocator.Reload(viewModel, param, parameterValues, savedState, navigationArgs).ConfigureAwait(false);

                return viewModelReload;
            }
            catch (Exception ex)
            {
                throw new MvxException($"Failed to reload a previously created created ViewModel for type {request.ViewModelType} from locator {viewModelLocator.GetType().Name} - check InnerException for more information", ex);
            }
        }

        public async ValueTask<IMvxViewModel> LoadViewModel(MvxViewModelRequest request, IMvxBundle? savedState, IMvxNavigateEventArgs? navigationArgs)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            if (request.ViewModelType == typeof(MvxNullViewModel))
            {
                return new MvxNullViewModel();
            }

            var viewModelLocator = FindViewModelLocator(request);

            var parameterValues = new MvxBundle(request.ParameterValues);
            try
            {
                var viewModel = await viewModelLocator.Load(request.ViewModelType, parameterValues, savedState, navigationArgs).ConfigureAwait(false);

                return viewModel;
            }
            catch (Exception ex)
            {
                throw new MvxException($"Failed to construct and initialize ViewModel for type {request.ViewModelType} from locator {viewModelLocator.GetType().Name} - check InnerException for more information", ex);
            }
        }

        public async ValueTask<IMvxViewModel> LoadViewModel<TParameter>(MvxViewModelRequest request, TParameter? param, IMvxBundle? savedState, IMvxNavigateEventArgs? navigationArgs)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            if (request.ViewModelType == typeof(MvxNullViewModel))
            {
                return new MvxNullViewModel();
            }

            var viewModelLocator = FindViewModelLocator(request);

            var parameterValues = new MvxBundle(request.ParameterValues);
            try
            {
                var viewModel = await viewModelLocator.Load(request.ViewModelType, param, parameterValues, savedState, navigationArgs).ConfigureAwait(false);
                return viewModel;
            }
            catch (Exception ex)
            {
                throw new MvxException($"Failed to construct and initialize ViewModel for type {request.ViewModelType} from locator {viewModelLocator.GetType().Name} - check InnerException for more information", ex);
            }
        }

        [Obsolete("Quitar la sobrecarga de localizadores. Todo por Roslyn.")]
        private IMvxViewModelLocator FindViewModelLocator(MvxViewModelRequest request)
        {
            var viewModelLocator = LocatorCollection.FindViewModelLocator(request);

            if (viewModelLocator == null)
            {
                throw new MvxException($"There's no viewmodel locator registered for {request.ViewModelType}");
            }

            return viewModelLocator;
        }
    }
}
