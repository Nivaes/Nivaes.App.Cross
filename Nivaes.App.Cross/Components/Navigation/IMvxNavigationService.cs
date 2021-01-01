﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross.Navigation
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;
    using Nivaes.App.Cross.ViewModels;

    public delegate void BeforeNavigateEventHandler(object sender, IMvxNavigateEventArgs e);

    public delegate void AfterNavigateEventHandler(object sender, IMvxNavigateEventArgs e);

    public delegate void BeforeCloseEventHandler(object sender, IMvxNavigateEventArgs e);

    public delegate void AfterCloseEventHandler(object sender, IMvxNavigateEventArgs e);

    public delegate void BeforeChangePresentationEventHandler(object sender, ChangePresentationEventArgs e);

    public delegate void AfterChangePresentationEventHandler(object sender, ChangePresentationEventArgs e);

    /// <summary>
    /// Allows for Task and URI based navigation in MvvmCross
    /// </summary>
    public interface IMvxNavigationService
    {
        public event BeforeNavigateEventHandler? BeforeNavigate;

        public event AfterNavigateEventHandler? AfterNavigate;

        public event BeforeCloseEventHandler? BeforeClose;

        public event AfterCloseEventHandler? AfterClose;

        public event BeforeChangePresentationEventHandler? BeforeChangePresentation;

        public event AfterChangePresentationEventHandler? AfterChangePresentation;

        /// <summary>
        /// Loads all navigation routes based on the referenced assemblies
        /// </summary>
        /// <param name="assemblies">The assemblies that should be indexed for routes</param>
        void LoadRoutes(IEnumerable<Assembly> assemblies);

        /// <summary>
        /// Navigates to an instance of a ViewModel
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="presentationBundle"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>Boolean indicating successful navigation</returns>
        ValueTask<bool> Navigate(IMvxViewModel viewModel, IMvxBundle? presentationBundle = null, CancellationToken? cancellationToken = default);

        /// <summary>
        /// Navigates to an instance of a ViewModel and passes TParameter
        /// </summary>
        /// <typeparam name="TParameter"></typeparam>
        /// <param name="viewModel"></param>
        /// <param name="param"></param>
        /// <param name="presentationBundle"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>Boolean indicating successful navigation</returns>
        ValueTask<bool> Navigate<TParameter>(IMvxViewModel<TParameter> viewModel, TParameter param, IMvxBundle? presentationBundle = null, CancellationToken? cancellationToken = default);

        /// <summary>
        /// Navigates to an instance of a ViewModel and returns TResult
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="viewModel"></param>
        /// <param name="presentationBundle"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        ValueTask<TResult> Navigate<TResult>(IMvxViewModelResult<TResult> viewModel, IMvxBundle? presentationBundle = null, CancellationToken? cancellationToken = default);

        /// <summary>
        /// Navigates to an instance of a ViewModel passes TParameter and returns TResult
        /// </summary>
        /// <typeparam name="TParameter"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="viewModel"></param>
        /// <param name="param"></param>
        /// <param name="presentationBundle"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        ValueTask<TResult> Navigate<TParameter, TResult>(IMvxViewModel<TParameter, TResult> viewModel, TParameter param, IMvxBundle? presentationBundle = null, CancellationToken? cancellationToken = default);

        /// <summary>
        /// Navigates to a ViewModel Type
        /// </summary>
        /// <param name="viewModelType"></param>
        /// <param name="presentationBundle"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>Boolean indicating successful navigation</returns>
        ValueTask<bool> Navigate(Type viewModelType, IMvxBundle? presentationBundle = null, CancellationToken? cancellationToken = default);

        /// <summary>
        /// Navigates to a ViewModel Type and passes TParameter
        /// </summary>
        /// <typeparam name="TParameter"></typeparam>
        /// <param name="viewModelType"></param>
        /// <param name="param"></param>
        /// <param name="presentationBundle"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>Boolean indicating successful navigation</returns>
        ValueTask<bool> Navigate<TParameter>(Type viewModelType, TParameter param, IMvxBundle? presentationBundle = null, CancellationToken? cancellationToken = default);

        /// <summary>
        /// Navigates to a ViewModel Type passes and returns TResult
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="viewModelType"></param>
        /// <param name="presentationBundle"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        ValueTask<TResult> Navigate<TResult>(Type viewModelType, IMvxBundle? presentationBundle = null, CancellationToken? cancellationToken = default);

        /// <summary>
        /// Navigates to a ViewModel Type passes TParameter and returns TResult
        /// </summary>
        /// <typeparam name="TParameter"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="viewModelType"></param>
        /// <param name="param"></param>
        /// <param name="presentationBundle"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        ValueTask<TResult> Navigate<TParameter, TResult>(Type viewModelType, TParameter param, IMvxBundle? presentationBundle = null, CancellationToken? cancellationToken = default);

        /// <summary>
        /// Translates the provided Uri to a ViewModel request and dispatches it.
        /// </summary>
        /// <param name="path">URI to route</param>
        /// <param name="presentationBundle"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>Boolean indicating successful navigation</returns>
        ValueTask<bool> Navigate(string path, IMvxBundle? presentationBundle = null, CancellationToken? cancellationToken = default);

        /// <summary>
        /// Translates the provided Uri to a ViewModel request and dispatches it.
        /// </summary>
        /// <typeparam name="TParameter"></typeparam>
        /// <param name="path"></param>
        /// <param name="param"></param>
        /// <param name="presentationBundle"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>Boolean indicating successful navigation</returns>
        ValueTask<bool> Navigate<TParameter>(string path, TParameter param, IMvxBundle? presentationBundle = null, CancellationToken? cancellationToken = default);

        /// <summary>
        /// Translates the provided Uri to a ViewModel request and dispatches it.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="path"></param>
        /// <param name="presentationBundle"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        ValueTask<TResult> Navigate<TResult>(string path, IMvxBundle? presentationBundle = null, CancellationToken? cancellationToken = default);

        /// <summary>
        /// Translates the provided Uri to a ViewModel request and dispatches it.
        /// </summary>
        /// <typeparam name="TParameter"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="path"></param>
        /// <param name="param"></param>
        /// <param name="presentationBundle"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        ValueTask<TResult> Navigate<TParameter, TResult>(string path, TParameter param, IMvxBundle? presentationBundle = null, CancellationToken? cancellationToken = default);

        ValueTask<bool> Navigate<TViewModel>(IMvxBundle? presentationBundle = null, CancellationToken? cancellationToken = default)
            where TViewModel : IMvxViewModel;

        ValueTask<bool> Navigate<TViewModel, TParameter>(TParameter param, IMvxBundle? presentationBundle = null, CancellationToken? cancellationToken = default)
            where TViewModel : IMvxViewModel<TParameter>;

        ValueTask<TResult> Navigate<TViewModel, TResult>(IMvxBundle? presentationBundle = null, CancellationToken? cancellationToken = default)
            where TViewModel : IMvxViewModelResult<TResult>;

        ValueTask<TResult> Navigate<TViewModel, TParameter, TResult>(TParameter param, IMvxBundle? presentationBundle = null, CancellationToken? cancellationToken = default)
            where TViewModel : IMvxViewModel<TParameter, TResult>;

        /// <summary>
        /// Verifies if the provided Uri can be routed to a ViewModel request.
        /// </summary>
        /// <param name="path">URI to route</param>
        /// <returns>True if the uri can be routed or false if it cannot.</returns>
        ValueTask<bool> CanNavigate(string path);

        /// <summary>
        /// Verifies if the provided viewmodel is available
        /// </summary>
        /// <returns>True if the ViewModel is available</returns>
        ValueTask<bool> CanNavigate<TViewModel>() where TViewModel : IMvxViewModel;

        /// <summary>
        /// Verifies if the provided viewmodel is available
        /// </summary>
        /// <param name="viewModelType">ViewModel type to check</param>
        /// <returns>True if the ViewModel is available</returns>
        ValueTask<bool> CanNavigate(Type viewModelType);

        /// <summary>
        /// Closes the View attached to the ViewModel
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        ValueTask<bool> Close(IMvxViewModel viewModel, CancellationToken? cancellationToken = default);

        /// <summary>
        /// Closes the View attached to the ViewModel and returns a result to the underlaying ViewModel
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="viewModel"></param>
        /// <param name="result"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        ValueTask<bool> Close<TResult>(IMvxViewModelResult<TResult> viewModel, TResult result, CancellationToken? cancellationToken = default);

        /// <summary>
        /// Dispatches a ChangePresentation with Hint
        /// </summary>
        /// <param name="hint"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        ValueTask<bool> ChangePresentation(MvxPresentationHint hint, CancellationToken? cancellationToken = default);
    }
}
