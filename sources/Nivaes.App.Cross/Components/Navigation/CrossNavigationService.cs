namespace Nivaes.App.Cross
{
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;
    using System.Text.RegularExpressions;
    using Microsoft.Extensions.Logging;
    using MvvmCross.Core;
    using MvvmCross.Exceptions;
    using MvvmCross.IoC;
    using MvvmCross.Logging;
    using MvvmCross.ViewModels;

    /// <inheritdoc cref="ICrossNavigationService"/>
    public class CrossNavigationService 
        : ICrossNavigationService
    {
        private readonly IMvxIoCProvider _iocProvider;

        private readonly Lazy<ILogger?> _log = new(() =>
            MvxLogHost.GetLog<CrossNavigationService>());

        public ICrossViewDispatcher ViewDispatcher { get; }

        protected Lazy<ICrossViewsContainer?> ViewsContainer { get; }

        protected Dictionary<Regex, Type> Routes { get; } = new();

        protected ICrossViewModelLoader ViewModelLoader { get; set; }

        public event EventHandler<ICrossNavigateEventArgs>? WillNavigate;

        public event EventHandler<ICrossNavigateEventArgs>? DidNavigate;

        public event EventHandler<ICrossNavigateEventArgs>? WillClose;

        public event EventHandler<ICrossNavigateEventArgs>? DidClose;

        public event EventHandler<ChangePresentationEventArgs>? WillChangePresentation;

        public event EventHandler<ChangePresentationEventArgs>? DidChangePresentation;

        public CrossNavigationService(
            ICrossViewModelLoader viewModelLoader,
            ICrossViewDispatcher viewDispatcher,
            IMvxIoCProvider iocProvider)
        {
            _iocProvider = iocProvider;

            ViewModelLoader = viewModelLoader;
            ViewDispatcher = viewDispatcher;
            ViewsContainer = new Lazy<ICrossViewsContainer?>(() => _iocProvider.Resolve<ICrossViewsContainer>());
        }

        public void LoadRoutes(IEnumerable<Assembly> assemblies)
        {
            ArgumentNullException.ThrowIfNull(assemblies);

            Routes.Clear();
            foreach (var routeAttr in
                     assemblies.SelectMany(a => a.GetCustomAttributes<CrossNavigationAttribute>()))
            {
                Routes.Add(new Regex(routeAttr.UriRegex,
                        RegexOptions.CultureInvariant | RegexOptions.IgnoreCase | RegexOptions.Singleline),
                    routeAttr.ViewModelOrFacade);
            }
        }

        protected virtual bool TryGetRoute(string path, out KeyValuePair<Regex, Type> entry)
        {
            ArgumentNullException.ThrowIfNull(path);

            try
            {
                var matches = Routes.Where(t => t.Key.IsMatch(path)).ToList();

                switch (matches.Count)
                {
                    case 0:
                        entry = default;
                        _log.Value?.Log(LogLevel.Trace, "Unable to find routing for {Path}", path);
                        return false;

                    case 1:
                        entry = matches[0];
                        return true;
                }

                var directMatch = matches.Where(t => t.Key.Match(path).Groups.Count == 1).ToList();

                if (directMatch.Count == 1)
                {
                    entry = directMatch[0];
                    return true;
                }

                _log.Value?.Log(LogLevel.Warning, "The following regular expressions match the provided url ({Count}), each RegEx must be unique (otherwise try using IMvxRoutingFacade): {Matches}",
                    matches.Count - 1,
                    string.Join(", ", matches.Select(t => t.Key.ToString())));

                // there is more than one match
                entry = default;
                return false;
            }
            catch (Exception ex)
            {
                _log.Value?.Log(LogLevel.Error, ex, "Unable to determine routability");
                entry = default;
                return false;
            }
        }

        protected virtual IDictionary<string, string> BuildParamDictionary(Regex regex, Match match)
        {
            ArgumentNullException.ThrowIfNull(regex);
            ArgumentNullException.ThrowIfNull(match);

            var paramDict = new Dictionary<string, string>();

            for (var i = 1 /* 0 == Match itself */; i < match.Groups.Count; i++)
            {
                var group = match.Groups[i];
                var name = regex.GroupNameFromNumber(i);
                var value = group.Value;
                paramDict.Add(name, value);
            }
            return paramDict;
        }

        [RequiresUnreferencedCode("This method uses reflection which may not be preserved during trimming")]
        protected virtual async Task<CrossViewModelInstanceRequest> NavigationRouteRequest(
            string path, ICrossBundle? presentationBundle = null)
        {
            ArgumentNullException.ThrowIfNull(path);

            if (!TryGetRoute(path, out var entry))
            {
                throw new CrossException($"Navigation route request could not be obtained for path: {path}");
            }

            var regex = entry.Key;
            var match = regex.Match(path);
            var paramDict = BuildParamDictionary(regex, match);
            var parameterValues = new CrossBundle(paramDict);

            var viewModelType = entry.Value;

            var request = new CrossViewModelInstanceRequest(viewModelType)
            {
                PresentationValues = presentationBundle?.SafeGetData(),
                ParameterValues = parameterValues.SafeGetData()
            };

            if (viewModelType.GetInterfaces().Contains(typeof(ICrossNavigationFacade)))
            {
                var facade = (ICrossNavigationFacade)_iocProvider.IoCConstruct(viewModelType);

                try
                {
                    var facadeRequest = await facade.BuildViewModelRequest(path, paramDict).ConfigureAwait(false);
                    if (facadeRequest == null)
                    {
                        throw new CrossException($"{nameof(CrossNavigationService)}: Facade did not return a valid {nameof(CrossViewModelRequest)}.");
                    }

                    request.ViewModelType = facadeRequest.ViewModelType;

                    if (facadeRequest.ParameterValues != null)
                    {
                        request.ParameterValues = facadeRequest.ParameterValues;
                    }

                    if (facadeRequest is CrossViewModelInstanceRequest instanceRequest)
                    {
                        request.ViewModelInstance = instanceRequest.ViewModelInstance ?? ViewModelLoader.LoadViewModel(request, null);
                    }
                    else
                    {
                        request.ViewModelInstance = ViewModelLoader.LoadViewModel(request, null);
                    }
                }
                catch (Exception ex)
                {
                    throw ex.Wrap($"{nameof(CrossNavigationService)}: Exception thrown while processing URL: {path} with RoutingFacade: {viewModelType}");
                }
            }
            else
            {
                request.ViewModelInstance = ViewModelLoader.LoadViewModel(request, null);
            }

            return request;
        }

        [RequiresUnreferencedCode("This method uses reflection which may not be preserved during trimming")]
        protected async Task<CrossViewModelInstanceRequest> NavigationRouteRequest<TParameter>(
            string path, TParameter param, ICrossBundle? presentationBundle = null)
        {
            ArgumentNullException.ThrowIfNull(path);
            ArgumentNullException.ThrowIfNull(param);

            if (!TryGetRoute(path, out var entry))
            {
                throw new CrossException($"Navigation route request could not be obtained for path: {path}");
            }

            var regex = entry.Key;
            var match = regex.Match(path);
            var paramDict = BuildParamDictionary(regex, match);
            var parameterValues = new CrossBundle(paramDict);

            var viewModelType = entry.Value;

            var request = new CrossViewModelInstanceRequest(viewModelType)
            {
                PresentationValues = presentationBundle?.SafeGetData(),
                ParameterValues = parameterValues.SafeGetData()
            };

            if (viewModelType.GetInterfaces().Contains(typeof(ICrossNavigationFacade)))
            {
                var facade = (ICrossNavigationFacade)_iocProvider.IoCConstruct(viewModelType);

                try
                {
                    var facadeRequest = await facade.BuildViewModelRequest(path, paramDict).ConfigureAwait(false);
                    if (facadeRequest == null)
                    {
                        throw new CrossException($"{nameof(CrossNavigationService)}: Facade did not return a valid {nameof(CrossViewModelRequest)}.");
                    }

                    request.ViewModelType = facadeRequest.ViewModelType;

                    if (facadeRequest.ParameterValues != null)
                    {
                        request.ParameterValues = facadeRequest.ParameterValues;
                    }

                    request.ViewModelInstance = ViewModelLoader.LoadViewModel(request, param, null);
                }
                catch (Exception ex)
                {
                    ex.Wrap($"{nameof(CrossNavigationService)}: Exception thrown while processing URL: {path} with RoutingFacade: {viewModelType}");
                }
            }
            else
            {
                request.ViewModelInstance = ViewModelLoader.LoadViewModel(request, param, null);
            }

            return request;
        }

        public virtual Task<bool> CanNavigate(string path)
        {
            return Task.FromResult(TryGetRoute(path, out _));
        }

        public virtual Task<bool> CanNavigate<TViewModel>()
            where TViewModel : ICrossViewModel
        {
            return Task.FromResult(ViewsContainer.Value?.GetViewType(typeof(TViewModel)) != null);
        }

        public virtual Task<bool> CanNavigate(Type viewModelType)
        {
            return Task.FromResult(ViewsContainer.Value?.GetViewType(viewModelType) != null);
        }

        protected virtual async Task<bool> Navigate(CrossViewModelRequest request, ICrossViewModel viewModel,
            ICrossBundle? presentationBundle = null, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(request);
            ArgumentNullException.ThrowIfNull(viewModel);

            var args = new CrossNavigateEventArgs(viewModel, NavigationMode.Show, cancellationToken);
            OnWillNavigate(this, args);

            if (args.Cancel)
                return false;

            var hasNavigated = await ViewDispatcher.ShowViewModel(request).ConfigureAwait(false);
            if (!hasNavigated)
                return false;

            if (viewModel.InitializeTask?.Task != null)
                await viewModel.InitializeTask.Task.ConfigureAwait(false);

            OnDidNavigate(this, args);
            return true;
        }

        [RequiresUnreferencedCode("This method uses reflection which may not be preserved during trimming")]
        public virtual async Task<bool> Navigate(
            string path, ICrossBundle? presentationBundle = null, CancellationToken cancellationToken = default)
        {
            var request = await NavigationRouteRequest(path, presentationBundle).ConfigureAwait(false);
            if (request.ViewModelInstance == null)
            {
                _log.Value?.Log(LogLevel.Warning, "Navigation Route Request doesn't have a ViewModelInstance");
                return false;
            }

            return await Navigate(request, request.ViewModelInstance, presentationBundle, cancellationToken).ConfigureAwait(false);
        }

        [RequiresUnreferencedCode("This method uses reflection which may not be preserved during trimming")]
        public virtual async Task<bool> Navigate<TParameter>(
                string path,
                TParameter param,
                ICrossBundle? presentationBundle = null,
                CancellationToken cancellationToken = default)
        {
            var request = await NavigationRouteRequest(path, param, presentationBundle).ConfigureAwait(false);
            if (request.ViewModelInstance == null)
            {
                _log.Value?.Log(LogLevel.Warning, "Navigation Route Request doesn't have a ViewModelInstance");
                return false;
            }
            return await Navigate(request, request.ViewModelInstance, presentationBundle, cancellationToken).ConfigureAwait(false);
        }

        public virtual Task<bool> Navigate(
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type viewModelType,
            ICrossBundle? presentationBundle = null,
            CancellationToken cancellationToken = default)
        {
            var request = new CrossViewModelInstanceRequest(viewModelType)
            {
                PresentationValues = presentationBundle?.SafeGetData()
            };
            request.ViewModelInstance = ViewModelLoader.LoadViewModel(request, null);
            return Navigate(request, request.ViewModelInstance, presentationBundle, cancellationToken);
        }

        public virtual Task<bool> Navigate<TParameter>(
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type viewModelType,
            TParameter param,
            ICrossBundle? presentationBundle = null,
            CancellationToken cancellationToken = default)
        {
            var request = new CrossViewModelInstanceRequest(viewModelType)
            {
                PresentationValues = presentationBundle?.SafeGetData()
            };
            request.ViewModelInstance = ViewModelLoader.LoadViewModel(request, param, null);
            return Navigate(request, request.ViewModelInstance, presentationBundle, cancellationToken);
        }

        public virtual Task<bool> Navigate<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TViewModel>(
            ICrossBundle? presentationBundle = null, CancellationToken cancellationToken = default)
            where TViewModel : ICrossViewModel
        {
            return Navigate(typeof(TViewModel), presentationBundle, cancellationToken);
        }

        public virtual Task<bool> Navigate<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TViewModel, TParameter>(
            TParameter param, ICrossBundle? presentationBundle = null, CancellationToken cancellationToken = default)
            where TViewModel : ICrossViewModel<TParameter>
        {
            return Navigate(typeof(TViewModel), param, presentationBundle, cancellationToken);
        }

        public virtual Task<bool> Navigate(
            ICrossViewModel viewModel, ICrossBundle? presentationBundle = null, CancellationToken cancellationToken = default)
        {
            var request = new CrossViewModelInstanceRequest(viewModel) { PresentationValues = presentationBundle?.SafeGetData() };
            ViewModelLoader.ReloadViewModel(viewModel, request, null);
            return Navigate(request, viewModel, presentationBundle, cancellationToken);
        }

        public virtual Task<bool> Navigate<TParameter>(ICrossViewModel<TParameter> viewModel, TParameter param,
            ICrossBundle? presentationBundle = null, CancellationToken cancellationToken = default)
        {
            var request = new CrossViewModelInstanceRequest(viewModel) { PresentationValues = presentationBundle?.SafeGetData() };
            ViewModelLoader.ReloadViewModel(viewModel, param, request, null);
            return Navigate(request, viewModel, presentationBundle, cancellationToken);
        }

        public virtual async Task<bool> ChangePresentation(
            CrossPresentationHint hint, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(hint);

            _log.Value?.Log(LogLevel.Trace, "Requesting presentation change");
            var args = new ChangePresentationEventArgs(hint, cancellationToken);
            OnWillChangePresentation(this, args);

            if (args.Cancel)
                return false;

            var result = await ViewDispatcher.ChangePresentation(hint).ConfigureAwait(false);

            args.Result = result;
            OnDidChangePresentation(this, args);

            return result;
        }

        public virtual async Task<bool> Close(ICrossViewModel viewModel, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(viewModel);

            var args = new CrossNavigateEventArgs(viewModel, NavigationMode.Close, cancellationToken);
            OnWillClose(this, args);

            if (args.Cancel)
                return false;

            var close = await ViewDispatcher.ChangePresentation(new CrossClosePresentationHint(viewModel)).ConfigureAwait(false);
            OnDidClose(this, args);

            return close;
        }

        protected virtual void OnWillNavigate(object sender, ICrossNavigateEventArgs e)
        {
            WillNavigate?.Invoke(sender, e);
        }

        protected virtual void OnDidNavigate(object sender, ICrossNavigateEventArgs e)
        {
            DidNavigate?.Invoke(sender, e);
        }

        protected virtual void OnWillClose(object sender, ICrossNavigateEventArgs e)
        {
            WillClose?.Invoke(sender, e);
        }

        protected virtual void OnDidClose(object sender, ICrossNavigateEventArgs e)
        {
            DidClose?.Invoke(sender, e);
        }

        protected virtual void OnWillChangePresentation(object sender, ChangePresentationEventArgs e)
        {
            WillChangePresentation?.Invoke(sender, e);
        }

        protected virtual void OnDidChangePresentation(object sender, ChangePresentationEventArgs e)
        {
            DidChangePresentation?.Invoke(sender, e);
        }

        /// <summary>
        ///     Loads a view model targeting the window for the given source.
        /// </summary>
        /// <typeparam name="TViewModel">The viewmodel type.</typeparam>
        /// <typeparam name="TParameter">The parameter type.</typeparam>
        /// <param name="param">The parameter value.</param>
        /// <param name="source">
        ///     This is used to find the window to execute the navigate in.
        ///     This is usually the viewmodel instance which calls this method. 
        /// </param>
        /// <param name="presentationBundle">The presentation bungle.</param>
        /// <param name="cancellationToken">Any cancellation token.</param>
        /// <returns>True if navigation was successful.</returns>
        public virtual Task<bool> Navigate<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TViewModel, TParameter>(
            TParameter param, ICrossViewModel source, ICrossBundle? presentationBundle = null,
            CancellationToken cancellationToken = default)
                where TViewModel : ICrossViewModel<TParameter>
                where TParameter : notnull
        {
            return Navigate(typeof(TViewModel), param, source, presentationBundle, cancellationToken);
        }

        /// <summary>
        ///     Loads a view model targeting the window for the given source.
        /// </summary>
        /// <typeparam name="TParameter">The parameter</typeparam>
        /// <param name="viewModelType">The viewmodel type.</param>
        /// <param name="param">The parameter value.</param>
        /// <param name="source">
        ///     This is used to find the window to execute the navigate in.
        ///     This is usually the viewmodel instance which calls this method. 
        /// </param>
        /// <param name="presentationBundle">The presentation bungle.</param>
        /// <param name="cancellationToken">Any cancellation token.</param>
        /// <returns>True if navigation was successful.</returns>
        public virtual Task<bool> Navigate<TParameter>(
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type viewModelType,
            TParameter param,
            ICrossViewModel source,
            ICrossBundle? presentationBundle = null,
            CancellationToken cancellationToken = default)
                where TParameter : notnull
        {
            var mvxViewModelInstanceRequest = new CrossViewModelInstanceRequestWithSource(viewModelType, source)
            {
                PresentationValues = presentationBundle?.SafeGetData()
            };
            mvxViewModelInstanceRequest.ViewModelInstance = ViewModelLoader.LoadViewModel(mvxViewModelInstanceRequest, param, null);
            return NavigateAsync(mvxViewModelInstanceRequest, mvxViewModelInstanceRequest.ViewModelInstance, presentationBundle, cancellationToken);
        }

        /// <summary>
        ///     Navigates to a view for the given type.
        /// </summary>
        /// <param name="viewModelType">The type of the viewmodel to navigate to.</param>
        /// <param name="source">
        ///     This is used to find the window to execute the navigate in.
        ///     This is usually the viewmodel instance which calls this method. 
        /// </param>
        /// <param name="presentationBundle">A presentation bundle.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public virtual Task<bool> Navigate(
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type viewModelType,
            ICrossViewModel source,
            ICrossBundle? presentationBundle = null,
            CancellationToken cancellationToken = default)
        {
            var request = new CrossViewModelInstanceRequestWithSource(viewModelType, source)
            {
                PresentationValues = presentationBundle?.SafeGetData()
            };
            request.ViewModelInstance = ViewModelLoader.LoadViewModel(request, null);
            return NavigateAsync(request, request.ViewModelInstance, presentationBundle, cancellationToken);
        }

        /// <summary>
        ///     Navigates to the viewmodel for the given type.
        /// </summary>
        /// <typeparam name="TViewModel">The type of the viewmodel to navigate to.</typeparam>
        /// <param name="source">
        ///     This is used to find the window to execute the navigate in.
        ///     This is usually the viewmodel instance which calls this method. 
        /// </param>
        /// <param name="presentationBundle">The presentation bundle.</param>
        /// <param name="cancellationToken">Any cancellation token.</param>
        /// <returns>True if successful, false otherwise.</returns>
        public virtual Task<bool> Navigate<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TViewModel>(ICrossViewModel source,
            ICrossBundle? presentationBundle = null, CancellationToken cancellationToken = default)
            where TViewModel : ICrossViewModel
        {
            return Navigate(typeof(TViewModel), source, presentationBundle, cancellationToken);
        }

        /// <summary>
        ///     Navigates to a view for the given viewmodel.
        /// </summary>
        /// <param name="viewModel">The viewmodel to navigate to.</param>
        /// <param name="source">
        ///     This is used to find the window to execute the navigate in.
        ///     This is usually the viewmodel instance which calls this method. 
        /// </param>
        /// <param name="presentationBundle">The presentation bundle.</param>
        /// <param name="cancellationToken">Any cancellation token.</param>
        /// <returns>True if successful, false otherwise.</returns>
        [UnconditionalSuppressMessage("Trimming", "IL2072", Justification = "ViewModel types are preserved by the navigation infrastructure and GetType() is safe here.")]
        public virtual Task<bool> Navigate(
            ICrossViewModel viewModel, ICrossViewModel source, ICrossBundle? presentationBundle = null, CancellationToken cancellationToken = default)
        {
            var request = new CrossViewModelInstanceRequestWithSource(viewModel.GetType(), source) { PresentationValues = presentationBundle?.SafeGetData() };
            ViewModelLoader.ReloadViewModel(viewModel, request, null);
            return NavigateAsync(request, viewModel, presentationBundle, cancellationToken);
        }

        /// <summary>
        ///     Navigates to a view for the given viewmodel.
        /// </summary>
        /// <typeparam name="TParameter">The parameter type.</typeparam>
        /// <param name="viewModel">The viewmodel to navigate to.</param>
        /// <param name="param">Any parameters.</param>
        /// <param name="source">
        ///     This is used to find the window to execute the navigate in.
        ///     This is usually the viewmodel instance which calls this method. 
        /// </param>
        /// <param name="presentationBundle">The presentation bundle.</param>
        /// <param name="cancellationToken">Any cancellation token.</param>
        /// <returns>True if successful, false otherwise.</returns>
        [UnconditionalSuppressMessage("Trimming", "IL2072", Justification = "ViewModel types are preserved by the navigation infrastructure and GetType() is safe here.")]
        public virtual Task<bool> Navigate<TParameter>(ICrossViewModel<TParameter> viewModel, TParameter param, ICrossViewModel source,
            ICrossBundle? presentationBundle = null, CancellationToken cancellationToken = default)
            where TParameter : notnull
        {
            var request = new CrossViewModelInstanceRequestWithSource(viewModel.GetType(), source) { PresentationValues = presentationBundle?.SafeGetData() };
            ViewModelLoader.ReloadViewModel(viewModel, param, request, null);
            return NavigateAsync(request, viewModel, presentationBundle, cancellationToken);
        }

        /// <summary>
        ///     Shows the ViewModel for the given request.
        /// </summary>
        /// <param name="request">The request to show the viewmodel for.</param>
        /// <param name="viewModel">The viewmodel for the navigation arguments.</param>
        /// <param name="presentationBundle">The presentation bundle.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>True is successful. False otherwise.</returns>
        protected virtual async Task<bool> NavigateAsync(CrossViewModelRequest request, ICrossViewModel viewModel,
            ICrossBundle? presentationBundle = null, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(request);
            ArgumentNullException.ThrowIfNull(viewModel);

            var args = new CrossNavigateEventArgs(viewModel, NavigationMode.Show, cancellationToken);
            OnWillNavigate(this, args);

            if (args.Cancel)
            {
                return false;
            }

            bool hasNavigated = await ViewDispatcher.ShowViewModel(request).ConfigureAwait(false);
            if (!hasNavigated)
            {
                return false;
            }

            if (viewModel.InitializeTask?.Task != null)
            {
                await viewModel.InitializeTask.Task.ConfigureAwait(false);
            }

            OnDidNavigate(this, args);
            return true;
        }
    }
}