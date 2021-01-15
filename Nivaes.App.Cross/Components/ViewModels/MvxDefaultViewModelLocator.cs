// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross.ViewModels
{
    using System;
    using System.Threading.Tasks;
    using MvvmCross.Exceptions;
    using Nivaes.App.Cross;
    using Nivaes.App.Cross.Logging;
    using Nivaes.App.Cross.Navigation;

    public class MvxDefaultViewModelLocator
        : IMvxViewModelLocator
    {
        private IMvxNavigationService? mNavigationService;
        protected IMvxNavigationService NavigationService => mNavigationService ??= Mvx.IoCProvider.Resolve<IMvxNavigationService>();

        //private IMvxLogProvider? mLogProvider;
        //protected IMvxLogProvider LogProvider => mLogProvider ??= Mvx.IoCProvider.Resolve<IMvxLogProvider>();

        public MvxDefaultViewModelLocator()
            : this(null) { }

        public MvxDefaultViewModelLocator(IMvxNavigationService? navigationService)
        {
            if (navigationService != null)
                mNavigationService = navigationService;
        }

        public virtual async ValueTask<IMvxViewModel> Reload(IMvxViewModel viewModel,
                                            IMvxBundle? parameterValues,
                                            IMvxBundle? savedState,
                                            IMvxNavigateEventArgs? navigationArgs)
        {
            await RunViewModelLifecycle(viewModel, parameterValues, savedState, navigationArgs).ConfigureAwait(false);

            return viewModel;
        }

        public virtual async ValueTask<IMvxViewModel<TParameter>> Reload<TParameter>(IMvxViewModel<TParameter> viewModel,
                                                                    TParameter param,
                                                                    IMvxBundle? parameterValues,
                                                                    IMvxBundle? savedState,
                                                                    IMvxNavigateEventArgs? navigationArgs)
        {
            await RunViewModelLifecycle(viewModel, param, parameterValues, savedState, navigationArgs).ConfigureAwait(false);

            return viewModel;
        }

        public virtual async ValueTask<IMvxViewModel> Load(Type viewModelType,
                                          IMvxBundle? parameterValues,
                                          IMvxBundle? savedState,
                                          IMvxNavigateEventArgs? navigationArgs)
        {
            try
            {
                var viewModel = (IMvxViewModel)Mvx.IoCProvider.IoCConstruct(viewModelType);

                await RunViewModelLifecycle(viewModel, parameterValues, savedState, navigationArgs).ConfigureAwait(false);

                return viewModel;
            }
            catch (Exception ex)
            {
                throw new MvxException($"Problem creating viewModel of type {viewModelType?.Name ?? "-"}", ex);
            }
        }

        public virtual async ValueTask<IMvxViewModel<TParameter>> Load<TParameter>(Type viewModelType,
                                                                  TParameter param,
                                                                  IMvxBundle? parameterValues,
                                                                  IMvxBundle ?savedState,
                                                                  IMvxNavigateEventArgs? navigationArgs)
        {
            try
            {
                var viewModel = (IMvxViewModel<TParameter>)Mvx.IoCProvider.IoCConstruct(viewModelType);

                await RunViewModelLifecycle(viewModel, param, parameterValues, savedState, navigationArgs).ConfigureAwait(false);

                return viewModel;
            }
            catch (Exception ex)
            {
                throw new MvxException($"Problem creating viewModel of type {viewModelType?.Name ?? "-"}", ex);
            }

        }

        protected virtual void CallCustomInitMethods(IMvxViewModel viewModel, IMvxBundle? parameterValues)
        {
            viewModel.CallBundleMethods("Init", parameterValues);
        }

        protected virtual void CallReloadStateMethods(IMvxViewModel viewModel, IMvxBundle? savedState)
        {
            viewModel.CallBundleMethods("ReloadState", savedState);
        }

        protected async Task RunViewModelLifecycle(IMvxViewModel viewModel,
            IMvxBundle? parameterValues, IMvxBundle? savedState, IMvxNavigateEventArgs? navigationArgs)
        {
            if (viewModel == null) throw new ArgumentNullException(nameof(viewModel));

            try
            {
                CallCustomInitMethods(viewModel, parameterValues);
                if (navigationArgs?.Cancel == true)
                    return;

                if (savedState != null)
                {
                    CallReloadStateMethods(viewModel, savedState);
                    if (navigationArgs?.Cancel == true)
                        return;
                }

                await viewModel.Start().ConfigureAwait(false);
                if (navigationArgs?.Cancel == true)
                    return;

                await viewModel.Prepare().ConfigureAwait(false);
                if (navigationArgs?.Cancel == true)
                    return;

                viewModel.InitializeTask = MvxNotifyTask.Create(async () => await viewModel.Initialize().ConfigureAwait(false));
            }
            catch (Exception ex)
            {
                throw new MvxException($"Problem running viewModel lifecycle of type {viewModel.GetType().Name}", ex);
            }
        }

        protected async ValueTask RunViewModelLifecycle<TParameter>(IMvxViewModel<TParameter> viewModel,
            TParameter param, IMvxBundle? parameterValues, IMvxBundle? savedState, IMvxNavigateEventArgs? navigationArgs)
        {
            if (viewModel == null) throw new ArgumentNullException(nameof(viewModel));

            try
            {
                CallCustomInitMethods(viewModel, parameterValues);
                if (navigationArgs?.Cancel == true)
                    return;
                if (savedState != null)
                {
                    CallReloadStateMethods(viewModel, savedState);
                    if (navigationArgs?.Cancel == true)
                        return;
                }

                await viewModel.Start().ConfigureAwait(false);
                if (navigationArgs?.Cancel == true)
                    return;

                await viewModel.Prepare().ConfigureAwait(false);
                if (navigationArgs?.Cancel == true)
                    return;

                await viewModel.Prepare(param).ConfigureAwait(false);
                if (navigationArgs?.Cancel == true)
                    return;

                viewModel.InitializeTask = MvxNotifyTask.Create(async () => await viewModel.Initialize().ConfigureAwait(false));
            }
            catch (Exception ex)
            {
                throw new MvxException($"Problem running viewModel lifecycle of type {viewModel.GetType().Name}", ex);
            }
        }
    }
}
