using System.Data.Common;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection.Metadata;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using OpenTelemetry;
using OpenTelemetry.Resources;

namespace Nivaes.App.Cross;

/// <summary>
/// Navitage service.
/// </summary>
public sealed class CrossNavigationService
{
    #region Properties
    private ICrossViewDispatcher _viewDispatcher;

    private readonly ILogger _logger;
    #endregion

    #region Eventes
    /// <summary>
    /// Event that triggers right before navigation happens
    /// </summary>
    public event EventHandler<ICrossNavigateEventArgs>? WillNavigate;

    /// <summary>
    /// Event that triggers right after navigation did occur
    /// </summary>
    public event EventHandler<ICrossNavigateEventArgs>? DidNavigate;

    /// <summary>
    /// Event that triggers right before Closing
    /// </summary>
    public event EventHandler<ICrossNavigateEventArgs>? WillClose;

    /// <summary>
    /// Event that triggers right after did happen
    /// </summary>
    public event EventHandler<ICrossNavigateEventArgs>? DidClose;

    /// <summary>
    /// Event that triggers when presentation will change
    /// </summary>
    public event EventHandler<ChangePresentationEventArgs>? WillChangePresentation;

    /// <summary>
    /// Event that triggers when presentation changed
    /// </summary>
    public event EventHandler<ChangePresentationEventArgs>? DidChangePresentation;
    #endregion

    public CrossNavigationService(
        ICrossViewDispatcher viewDispatcher,
        ILogger<CrossNavigationService> logger)
    {
        _viewDispatcher = viewDispatcher;
        _logger = logger;
    }

    public IDictionary<string, string> BuildParamDictionary(Regex regex, Match match)
    {
        ArgumentNullException.ThrowIfNull(regex);
        ArgumentNullException.ThrowIfNull(match);

        var paramDict = new Dictionary<string, string>();

        for (var i = 1 ; i < match.Groups.Count; i++)
        {
            var group = match.Groups[i];
            var name = regex.GroupNameFromNumber(i);
            var value = group.Value;
            paramDict.Add(name, value);
        }
        return paramDict;
    }

    /// <summary>
    /// Verifies if the provided viewmodel is available
    /// </summary>
    /// <returns>True if the ViewModel is available</returns>
    public Task<bool> CanNavigate<TViewModel>()
        where TViewModel : ICrossViewModel
    {
        var viewType = Singleton<ViewModelViewsKeyContainerManager>.Instance
            .GetValue(typeof(TViewModel));

        return Task.FromResult(viewType != null);
    }

    public Task<bool> Navigate<TViewModel>(
            ICrossBundle? presentationBundle = null, 
            CancellationToken cancellationToken = default)
        where TViewModel : ICrossViewModel
    {
        var request = new ViewModelRequest<TViewModel>()
        {
            PresentationValues = presentationBundle?.SafeGetData()
        };

        return Navigate<TViewModel>(request, presentationBundle, cancellationToken);
    }

    private async Task<bool> Navigate<TViewModel>(
            IViewModelRequest request, 
            ICrossBundle? presentationBundle = null, 
            CancellationToken cancellationToken = default)
        where TViewModel : ICrossViewModel
    {
        ArgumentNullException.ThrowIfNull(request);

        var args = new CrossNavigateEventArgs(request.ViewModel, NavigationMode.Show, cancellationToken);
        OnWillNavigate(this, args);

        if (args.Cancel)
            return false;

        var hasNavigated = await _viewDispatcher.ShowViewModel(request).ConfigureAwait(false);
        if (!hasNavigated)
            return false;

        if (request.ViewModel.InitializeTask?.Task != null)
            await request.ViewModel.InitializeTask.Task.ConfigureAwait(false);

        OnDidNavigate(this, args);
        return true;
    }

    public Task<bool> Navigate<TViewModel, TParameter>(
            TParameter parameter,
            ICrossBundle? presentationBundle = null, 
            CancellationToken cancellationToken = default)
        where TViewModel : ICrossViewModel<TParameter>
        where TParameter : notnull
    {
        ArgumentNullException.ThrowIfNull(parameter);

        var request = new ViewModelRequestParameter<TViewModel, TParameter>(parameter)
        {
            Parameter = parameter,
            PresentationValues = presentationBundle?.SafeGetData()
        };
        return Navigate<TViewModel, TParameter>(parameter, request, request.ViewModel, presentationBundle, cancellationToken);
    }

    private async Task<bool> Navigate<TViewModel, TParameter>(
            TParameter parameter,
            IViewModelRequest request, 
            ICrossViewModel viewModel,
            ICrossBundle? presentationBundle = null, 
            CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(parameter);
        ArgumentNullException.ThrowIfNull(request);

        var args = new CrossNavigateEventArgs(viewModel, NavigationMode.Show, cancellationToken);
        OnWillNavigate(this, args);

        if (args.Cancel)
            return false;

        var hasNavigated = await _viewDispatcher.ShowViewModel(request).ConfigureAwait(false);
        if (!hasNavigated)
            return false;

        if (viewModel.InitializeTask?.Task != null)
            await viewModel.InitializeTask.Task.ConfigureAwait(false);

        OnDidNavigate(this, args);
        return true;
    }

    /// <summary>
    ///     Loads a view model targeting the window for the given source.
    /// </summary>
    /// <typeparam name="TViewModel">The viewmodel type.</typeparam>
    /// <typeparam name="TParameter">The parameter type.</typeparam>
    /// <param name="parameter">The parameter value.</param>
    /// <param name="source">
    ///     This is used to find the window to execute the navigate in.
    ///     This is usually the viewmodel instance which calls this method. 
    /// </param>
    /// <param name="presentationBundle">The presentation bungle.</param>
    /// <param name="cancellationToken">Any cancellation token.</param>
    /// <returns>True if navigation was successful.</returns>
    public Task<bool> Navigate<TViewModel, TParameter>(
            TParameter parameter, 
            ICrossViewModel source, 
            ICrossBundle? presentationBundle = null,
            CancellationToken cancellationToken = default)
        where TViewModel : ICrossViewModel<TParameter>
        where TParameter : notnull
    {
        ArgumentNullException.ThrowIfNull(parameter);
        ArgumentNullException.ThrowIfNull(source);

        throw new NotImplementedException("No se para que sirve source y hay que unificar CrossViewModelInstanceRequestWithSource con ViewModelReques");

        //var mvxViewModelInstanceRequest = new CrossViewModelInstanceRequestWithSource(typeof(TViewModel), source)
        //{
        //    PresentationValues = presentationBundle?.SafeGetData()
        //};
        ////mvxViewModelInstanceRequest.ViewModelInstance = ViewModelLoader.LoadViewModel<TParameter>(mvxViewModelInstanceRequest, param, null);
        //return NavigateAsync(mvxViewModelInstanceRequest, mvxViewModelInstanceRequest.ViewModel, presentationBundle, cancellationToken);
    }

    public Task<TResult> Navigate<TViewModel, TResult>(ICrossBundle? presentationBundle = null, CancellationToken cancellationToken = default) where TViewModel : ICrossViewModelResult<TResult>
    {
        throw new NotImplementedException();
    }

    public Task<TResult> Navigate<TViewModel, TParameter, TResult>(TParameter param, ICrossBundle? presentationBundle = null, CancellationToken cancellationToken = default) where TViewModel : ICrossViewModel<TParameter, TResult>
    {
        ArgumentNullException.ThrowIfNull(param);

        throw new NotImplementedException();
    }

    /// <summary>
    /// Navigate from a Result Awaiting ViewModel to a Result Setting ViewModel determined by its type, with parameter
    /// </summary>
    /// <param name="fromViewModel">Result Awaiting ViewModel</param>
    /// <param name="resultViewModelManager">Result ViewModel Manager</param>
    /// <param name="parameter">ViewModel parameter</param>
    /// <param name="presentationBundle">(optional) presentation bundle</param>
    /// <param name="cancellationToken">(optional) CancellationToken to cancel the navigation</param>
    /// <typeparam name="TViewModel">Type of <see cref="ICrossResultSettingViewModel{TResult}"/> and <see cref="ICrossViewModel{TParameter}"/></typeparam>
    /// <typeparam name="TResult">Result awaited by Result Awaiting ViewModel and set by Result Setting ViewModel</typeparam>
    /// <returns>Boolean indicating successful navigation</returns>
    public async Task<bool> NavigateRegisteringToResult<TViewModel, TParameter, TResult>(
        ICrossResultAwaitingViewModel<TResult> fromViewModel,
        ICrossResultViewModelManager resultViewModelManager,
        TParameter parameter,
        ICrossBundle? presentationBundle = null,
        CancellationToken cancellationToken = default)
        where TViewModel : ICrossResultSettingViewModel<TResult>, ICrossViewModel<TParameter>
        where TParameter : notnull
    {
        ArgumentNullException.ThrowIfNull(fromViewModel);
        ArgumentNullException.ThrowIfNull(resultViewModelManager);
        ArgumentNullException.ThrowIfNull(parameter);

        bool navigated = await Navigate<TViewModel, TParameter>(parameter, presentationBundle, cancellationToken);
        if (navigated)
            fromViewModel.RegisterToResult(resultViewModelManager);
        return navigated;
    }

    /// <summary>
    /// Closes the View attached to the Result Setting ViewModel, with result
    /// </summary>
    /// <param name="viewModel">Result Setting ViewModel to close</param>
    /// <param name="result">Result set by Result Setting ViewModel</param>
    /// <param name="cancellationToken">(optional) CancellationToken to cancel the closing</param>
    /// <typeparam name="TViewModel">Type of <see cref="ICrossResultSettingViewModel{TResult}"/></typeparam>
    /// <typeparam name="TResult">Result set by Result Setting ViewModel</typeparam>
    /// <returns></returns>
    public async Task<bool> CloseSettingResult<TViewModel, TResult>(
        TViewModel viewModel,
        TResult result,
        CancellationToken cancellationToken = default)
        where TViewModel : ICrossResultSettingViewModel<TResult>, ICrossViewModel
    {
        ArgumentNullException.ThrowIfNull(viewModel);
        ArgumentNullException.ThrowIfNull(result);

        bool closed = await Close(viewModel, cancellationToken);
        if (closed)
            viewModel.SetResult(result);
        return closed;
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
    public Task<bool> Navigate<TViewModel>(ICrossViewModel source,
        ICrossBundle? presentationBundle = null, 
        CancellationToken cancellationToken = default)
        where TViewModel : ICrossViewModel
    {
        ArgumentNullException.ThrowIfNull(source);

        var request = new ViewModelRequestSource<TViewModel>(source)
        {
            PresentationValues = presentationBundle?.SafeGetData()
        };
        //request.ViewModel = ViewModelLoader.LoadViewModel(request, null);
        return NavigateAsync(request, request.ViewModel, presentationBundle, cancellationToken);
    }

    /// <summary>
    ///     Shows the ViewModel for the given request.
    /// </summary>
    /// <param name="request">The request to show the viewmodel for.</param>
    /// <param name="viewModel">The viewmodel for the navigation arguments.</param>
    /// <param name="presentationBundle">The presentation bundle.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>True is successful. False otherwise.</returns>
    private async Task<bool> NavigateAsync(
        IViewModelRequest request, ICrossViewModel viewModel,
        ICrossBundle? presentationBundle = null, CancellationToken cancellationToken = default)
    {
        var args = new CrossNavigateEventArgs(viewModel, NavigationMode.Show, cancellationToken);
        OnWillNavigate(this, args);

        if (args.Cancel)
        {
            return false;
        }

        bool hasNavigated = await _viewDispatcher.ShowViewModel(request).ConfigureAwait(false);
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

    public async Task<bool> ChangePresentation(CrossPresentationHint hint, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(hint);

        _logger.Log(LogLevel.Trace, "Requesting presentation change");
        var args = new ChangePresentationEventArgs(hint, cancellationToken);
        OnWillChangePresentation(this, args);

        if (args.Cancel)
            return false;

        var result = await _viewDispatcher.ChangePresentation(hint).ConfigureAwait(false);

        args.Result = result;
        OnDidChangePresentation(this, args);

        return result;
    }

    public async Task<bool> Close(ICrossViewModel viewModel, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(viewModel);

        var args = new CrossNavigateEventArgs(viewModel, NavigationMode.Close, cancellationToken);
        OnWillClose(this, args);

        if (args.Cancel)
            return false;

        var close = await _viewDispatcher.ChangePresentation(new CrossClosePresentationHint(viewModel)).ConfigureAwait(false);
        OnDidClose(this, args);

        return close;
    }

    private Task<bool> Close<TResult>(ICrossViewModelResult<TResult> viewModel, TResult result, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    #region EventsNavigate
    private void OnWillNavigate(object sender, ICrossNavigateEventArgs e)
    {
        WillNavigate?.Invoke(sender, e);
    }

    private void OnDidNavigate(object sender, ICrossNavigateEventArgs e)
    {
        DidNavigate?.Invoke(sender, e);
    }

    private void OnWillClose(object sender, ICrossNavigateEventArgs e)
    {
        WillClose?.Invoke(sender, e);
    }

    private void OnDidClose(object sender, ICrossNavigateEventArgs e)
    {
        DidClose?.Invoke(sender, e);
    }

    private void OnWillChangePresentation(object sender, ChangePresentationEventArgs e)
    {
        WillChangePresentation?.Invoke(sender, e);
    }

    private void OnDidChangePresentation(object sender, ChangePresentationEventArgs e)
    {
        DidChangePresentation?.Invoke(sender, e);
    }
    #endregion
}