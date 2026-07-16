using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross;

/// <inheritdoc cref="ICrossNavigationService"/>
public class CrossNavigationService
    : ICrossNavigationService
{
    private readonly ILogger _logger;

    public ICrossViewDispatcher ViewDispatcher { [DebuggerHidden] get; }

    protected ICrossViewsContainer? ViewsContainer { [DebuggerHidden] get; }

    protected Dictionary<Regex, Type> Routes { [DebuggerHidden] get; } = new();

    protected ICrossViewModelLoader ViewModelLoader { [DebuggerHidden] get; [DebuggerHidden] set; }

    public event EventHandler<ICrossNavigateEventArgs>? WillNavigate;

    public event EventHandler<ICrossNavigateEventArgs>? DidNavigate;

    public event EventHandler<ICrossNavigateEventArgs>? WillClose;

    public event EventHandler<ICrossNavigateEventArgs>? DidClose;

    public event EventHandler<ChangePresentationEventArgs>? WillChangePresentation;

    public event EventHandler<ChangePresentationEventArgs>? DidChangePresentation;

    public CrossNavigationService(
        ICrossViewModelLoader viewModelLoader,
        ICrossViewDispatcher viewDispatcher,
        ICrossViewsContainer crossViewsContainer,
        ILogger<CrossNavigationService> logger)
    {
        ViewModelLoader = viewModelLoader;
        ViewDispatcher = viewDispatcher;
        ViewsContainer = crossViewsContainer;
        _logger = logger;
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

    public virtual Task<bool> CanNavigate<TViewModel>()
        where TViewModel : ICrossViewModel
    {
        return Task.FromResult(ViewsContainer?.GetViewType(typeof(TViewModel)) != null);
    }

    public virtual Task<bool> CanNavigate(Type viewModelType)
    {
        return Task.FromResult(ViewsContainer?.GetViewType(viewModelType) != null);
    }

    protected virtual async Task<bool> Navigate<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TViewModel>(
        CrossViewModelRequest request, ICrossViewModel viewModel,
        ICrossBundle? presentationBundle = null, CancellationToken cancellationToken = default)
    {
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

    //[RequiresUnreferencedCode("This method uses reflection which may not be preserved during trimming")]
    //public virtual async Task<bool> Navigate(
    //    string path, ICrossBundle? presentationBundle = null, CancellationToken cancellationToken = default)
    //{
    //    var request = await NavigationRouteRequest(path, presentationBundle).ConfigureAwait(false);
    //    if (request.ViewModelInstance == null)
    //    {
    //        _log.Value?.Log(LogLevel.Warning, "Navigation Route Request doesn't have a ViewModelInstance");
    //        return false;
    //    }

    //    return await Navigate(request, request.ViewModelInstance, presentationBundle, cancellationToken).ConfigureAwait(false);
    //}

    //[RequiresUnreferencedCode("This method uses reflection which may not be preserved during trimming")]
    //public virtual async Task<bool> Navigate<TParameter>(
    //        string path,
    //        TParameter param,
    //        ICrossBundle? presentationBundle = null,
    //        CancellationToken cancellationToken = default)
    //{
    //    var request = await NavigationRouteRequest(path, param, presentationBundle).ConfigureAwait(false);
    //    if (request.ViewModelInstance == null)
    //    {
    //        _log.Value?.Log(LogLevel.Warning, "Navigation Route Request doesn't have a ViewModelInstance");
    //        return false;
    //    }
    //    return await Navigate(request, request.ViewModelInstance, presentationBundle, cancellationToken).ConfigureAwait(false);
    //}

    //public virtual Task<bool> Navigate(
    //    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type viewModelType,
    //    ICrossBundle? presentationBundle = null,
    //    CancellationToken cancellationToken = default)
    //{
    //    var request = new CrossViewModelInstanceRequest(viewModelType)
    //    {
    //        PresentationValues = presentationBundle?.SafeGetData()
    //    };
    //    request.ViewModelInstance = ViewModelLoader.LoadViewModel(request, null);
    //    return Navigate(request, request.ViewModelInstance, presentationBundle, cancellationToken);
    //}

    //public virtual Task<bool> Navigate<TParameter>(
    //    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type viewModelType,
    //    TParameter param,
    //    ICrossBundle? presentationBundle = null,
    //    CancellationToken cancellationToken = default)
    //{
    //    var request = new CrossViewModelInstanceRequest(viewModelType)
    //    {
    //        PresentationValues = presentationBundle?.SafeGetData()
    //    };
    //    request.ViewModelInstance = ViewModelLoader.LoadViewModel(request, param, null);
    //    return Navigate(request, request.ViewModelInstance, presentationBundle, cancellationToken);
    //}

    public virtual Task<bool> Navigate<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TViewModel>(
        ICrossBundle? presentationBundle = null, CancellationToken cancellationToken = default)
        where TViewModel : ICrossViewModel
    {
        var request = new CrossViewModelInstanceRequest(typeof(TViewModel))
        {
            PresentationValues = presentationBundle?.SafeGetData()
        };
        request.ViewModelInstance = ViewModelLoader.LoadViewModel(request, null);
        return Navigate<TViewModel>(request, request.ViewModelInstance, presentationBundle, cancellationToken);
    }

    public virtual Task<bool> Navigate<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TViewModel, TParameter>(
        TParameter param, ICrossBundle? presentationBundle = null, CancellationToken cancellationToken = default)
        where TViewModel : ICrossViewModel<TParameter>
    {
        var request = new CrossViewModelInstanceRequest(typeof(TViewModel))
        {
            PresentationValues = presentationBundle?.SafeGetData()
        };
        request.ViewModelInstance = ViewModelLoader.LoadViewModel<TParameter>(request, param, null);
        return Navigate<TViewModel>(request, request.ViewModelInstance, presentationBundle, cancellationToken);
    }

    //public virtual Task<bool> Navigate(
    //    ICrossViewModel viewModel, ICrossBundle? presentationBundle = null, CancellationToken cancellationToken = default)
    //{
    //    var request = new CrossViewModelInstanceRequest(viewModel) { PresentationValues = presentationBundle?.SafeGetData() };
    //    ViewModelLoader.ReloadViewModel(viewModel, request, null);
    //    return Navigate(request, viewModel, presentationBundle, cancellationToken);
    //}

    //public virtual Task<bool> Navigate<TParameter>(ICrossViewModel<TParameter> viewModel, TParameter param,
    //    ICrossBundle? presentationBundle = null, CancellationToken cancellationToken = default)
    //{
    //    var request = new CrossViewModelInstanceRequest(viewModel) { PresentationValues = presentationBundle?.SafeGetData() };
    //    ViewModelLoader.ReloadViewModel(viewModel, param, request, null);
    //    return Navigate(request, viewModel, presentationBundle, cancellationToken);
    //}

    public virtual async Task<bool> ChangePresentation(
        CrossPresentationHint hint, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(hint);

        _logger.Log(LogLevel.Trace, "Requesting presentation change");
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
    public Task<bool> Close<TResult>(ICrossViewModelResult<TResult> viewModel, TResult result, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
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
    public virtual Task<bool> Navigate<TViewModel, TParameter>(
        TParameter param, ICrossViewModel source, ICrossBundle? presentationBundle = null,
        CancellationToken cancellationToken = default)
            where TViewModel : ICrossViewModel<TParameter>
            where TParameter : notnull
    {
        //return Navigate(typeof(TViewModel), param, source, presentationBundle, cancellationToken);

        var mvxViewModelInstanceRequest = new CrossViewModelInstanceRequestWithSource(typeof(TViewModel), source)
        {
            PresentationValues = presentationBundle?.SafeGetData()
        };
        mvxViewModelInstanceRequest.ViewModelInstance = ViewModelLoader.LoadViewModel<TParameter>(mvxViewModelInstanceRequest, param, null);
        return NavigateAsync(mvxViewModelInstanceRequest, mvxViewModelInstanceRequest.ViewModelInstance, presentationBundle, cancellationToken);
    }

    public Task<TResult> Navigate<TViewModel, TResult>(ICrossBundle? presentationBundle = null, CancellationToken cancellationToken = default) where TViewModel : ICrossViewModelResult<TResult>
    {
        throw new NotImplementedException();
    }

    public Task<TResult> Navigate<TViewModel, TParameter, TResult>(TParameter param, ICrossBundle? presentationBundle = null, CancellationToken cancellationToken = default) where TViewModel : ICrossViewModel<TParameter, TResult>
    {
        throw new NotImplementedException();
    }

    ///// <summary>
    /////     Loads a view model targeting the window for the given source.
    ///// </summary>
    ///// <typeparam name="TParameter">The parameter</typeparam>
    ///// <param name="viewModelType">The viewmodel type.</param>
    ///// <param name="param">The parameter value.</param>
    ///// <param name="source">
    /////     This is used to find the window to execute the navigate in.
    /////     This is usually the viewmodel instance which calls this method. 
    ///// </param>
    ///// <param name="presentationBundle">The presentation bungle.</param>
    ///// <param name="cancellationToken">Any cancellation token.</param>
    ///// <returns>True if navigation was successful.</returns>
    //public virtual Task<bool> Navigate<TParameter>(
    //    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type viewModelType,
    //    TParameter param,
    //    ICrossViewModel source,
    //    ICrossBundle? presentationBundle = null,
    //    CancellationToken cancellationToken = default)
    //        where TParameter : notnull
    //{
    //    var mvxViewModelInstanceRequest = new CrossViewModelInstanceRequestWithSource(viewModelType, source)
    //    {
    //        PresentationValues = presentationBundle?.SafeGetData()
    //    };
    //    mvxViewModelInstanceRequest.ViewModelInstance = ViewModelLoader.LoadViewModel(mvxViewModelInstanceRequest, param, null);
    //    return NavigateAsync(mvxViewModelInstanceRequest, mvxViewModelInstanceRequest.ViewModelInstance, presentationBundle, cancellationToken);
    //}

    ///// <summary>
    /////     Navigates to a view for the given type.
    ///// </summary>
    ///// <param name="viewModelType">The type of the viewmodel to navigate to.</param>
    ///// <param name="source">
    /////     This is used to find the window to execute the navigate in.
    /////     This is usually the viewmodel instance which calls this method. 
    ///// </param>
    ///// <param name="presentationBundle">A presentation bundle.</param>
    ///// <param name="cancellationToken">The cancellation token.</param>
    ///// <returns></returns>
    //public virtual Task<bool> Navigate(
    //    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type viewModelType,
    //    ICrossViewModel source,
    //    ICrossBundle? presentationBundle = null,
    //    CancellationToken cancellationToken = default)
    //{
    //    var request = new CrossViewModelInstanceRequestWithSource(viewModelType, source)
    //    {
    //        PresentationValues = presentationBundle?.SafeGetData()
    //    };
    //    request.ViewModelInstance = ViewModelLoader.LoadViewModel(request, null);
    //    return NavigateAsync(request, request.ViewModelInstance, presentationBundle, cancellationToken);
    //}

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
        var request = new CrossViewModelInstanceRequestWithSource(typeof(TViewModel), source)
        {
            PresentationValues = presentationBundle?.SafeGetData()
        };
        request.ViewModelInstance = ViewModelLoader.LoadViewModel(request, null);
        return NavigateAsync(request, request.ViewModelInstance, presentationBundle, cancellationToken);
    }

    ///// <summary>
    /////     Navigates to a view for the given viewmodel.
    ///// </summary>
    ///// <param name="viewModel">The viewmodel to navigate to.</param>
    ///// <param name="source">
    /////     This is used to find the window to execute the navigate in.
    /////     This is usually the viewmodel instance which calls this method. 
    ///// </param>
    ///// <param name="presentationBundle">The presentation bundle.</param>
    ///// <param name="cancellationToken">Any cancellation token.</param>
    ///// <returns>True if successful, false otherwise.</returns>
    //[UnconditionalSuppressMessage("Trimming", "IL2072", Justification = "ViewModel types are preserved by the navigation infrastructure and GetType() is safe here.")]
    //public virtual Task<bool> Navigate(
    //    ICrossViewModel viewModel, ICrossViewModel source, ICrossBundle? presentationBundle = null, CancellationToken cancellationToken = default)
    //{
    //    var request = new CrossViewModelInstanceRequestWithSource(viewModel.GetType(), source) { PresentationValues = presentationBundle?.SafeGetData() };
    //    ViewModelLoader.ReloadViewModel(viewModel, request, null);
    //    return NavigateAsync(request, viewModel, presentationBundle, cancellationToken);
    //}

    ///// <summary>
    /////     Navigates to a view for the given viewmodel.
    ///// </summary>
    ///// <typeparam name="TParameter">The parameter type.</typeparam>
    ///// <param name="viewModel">The viewmodel to navigate to.</param>
    ///// <param name="param">Any parameters.</param>
    ///// <param name="source">
    /////     This is used to find the window to execute the navigate in.
    /////     This is usually the viewmodel instance which calls this method. 
    ///// </param>
    ///// <param name="presentationBundle">The presentation bundle.</param>
    ///// <param name="cancellationToken">Any cancellation token.</param>
    ///// <returns>True if successful, false otherwise.</returns>
    //[UnconditionalSuppressMessage("Trimming", "IL2072", Justification = "ViewModel types are preserved by the navigation infrastructure and GetType() is safe here.")]
    //public virtual Task<bool> Navigate<TParameter>(ICrossViewModel<TParameter> viewModel, TParameter param, ICrossViewModel source,
    //    ICrossBundle? presentationBundle = null, CancellationToken cancellationToken = default)
    //    where TParameter : notnull
    //{
    //    var request = new CrossViewModelInstanceRequestWithSource(viewModel.GetType(), source) { PresentationValues = presentationBundle?.SafeGetData() };
    //    ViewModelLoader.ReloadViewModel(viewModel, param, request, null);
    //    return NavigateAsync(request, viewModel, presentationBundle, cancellationToken);
    //}

    /// <summary>
    ///     Shows the ViewModel for the given request.
    /// </summary>
    /// <param name="request">The request to show the viewmodel for.</param>
    /// <param name="viewModel">The viewmodel for the navigation arguments.</param>
    /// <param name="presentationBundle">The presentation bundle.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>True is successful. False otherwise.</returns>
    protected virtual async Task<bool> NavigateAsync(
        CrossViewModelRequest request, ICrossViewModel viewModel,
        ICrossBundle? presentationBundle = null, CancellationToken cancellationToken = default)
    {
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