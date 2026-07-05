using System.Diagnostics.CodeAnalysis;

namespace Nivaes.App.Cross;

/// <summary>
/// Allows for Task and URI based navigation in MvvmCross
/// </summary>
public interface ICrossNavigationService
{
    /// <summary>
    /// Event that triggers right before navigation happens
    /// </summary>
    event EventHandler<ICrossNavigateEventArgs>? WillNavigate;

    /// <summary>
    /// Event that triggers right after navigation did occur
    /// </summary>
    event EventHandler<ICrossNavigateEventArgs>? DidNavigate;

    /// <summary>
    /// Event that triggers right before Closing
    /// </summary>
    event EventHandler<ICrossNavigateEventArgs>? WillClose;

    /// <summary>
    /// Event that triggers right after did happen
    /// </summary>
    event EventHandler<ICrossNavigateEventArgs>? DidClose;

    /// <summary>
    /// Event that triggers when presentation will change
    /// </summary>
    event EventHandler<ChangePresentationEventArgs>? WillChangePresentation;

    /// <summary>
    /// Event that triggers when presentation changed
    /// </summary>
    event EventHandler<ChangePresentationEventArgs>? DidChangePresentation;

    /// <summary>
    /// Verifies if the provided viewmodel is available
    /// </summary>
    /// <returns>True if the ViewModel is available</returns>
    Task<bool> CanNavigate<TViewModel>()
        where TViewModel : ICrossViewModel;

    /// <summary>
    /// Closes the View attached to the ViewModel
    /// </summary>
    /// <param name="viewModel"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> Close(ICrossViewModel viewModel, CancellationToken cancellationToken = default);

    /// <summary>
    /// Closes the View attached to the ViewModel and returns a result to the underlaying ViewModel
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="viewModel"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    Task<bool> Close<TResult>(ICrossViewModelResult<TResult> viewModel, TResult result, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Dispatches a ChangePresentation with Hint
    /// </summary>
    /// <param name="hint"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> ChangePresentation(CrossPresentationHint hint, CancellationToken cancellationToken = default);

    /// <summary>
    /// Navigate to a ViewModel determined by its type
    /// </summary>
    /// <param name="presentationBundle">(optional) presentation bundle</param>
    /// <param name="cancellationToken">CancellationToken to cancel the navigation</param>
    /// <typeparam name="TViewModel">Type of <see cref="ICrossViewModel"/></typeparam>
    /// <returns>Boolean indicating successful navigation</returns>
    Task<bool> Navigate<TViewModel>(
        ICrossBundle? presentationBundle = null, CancellationToken cancellationToken = default)
        where TViewModel : ICrossViewModel;

    /// <summary>
    /// Navigate to a ViewModel determined by its type, with parameter
    /// </summary>
    /// <param name="param">ViewModel parameter</param>
    /// <param name="presentationBundle">(optional) presentation bundle</param>
    /// <param name="cancellationToken">CancellationToken to cancel the navigation</param>
    /// <typeparam name="TViewModel">Type of <see cref="ICrossViewModel{Parameter}"/></typeparam>
    /// <typeparam name="TParameter">Parameter passed to ViewModel</typeparam>
    /// <returns>Boolean indicating successful navigation</returns>
    Task<bool> Navigate<TViewModel, TParameter>(
        TParameter param, ICrossBundle? presentationBundle = null, CancellationToken cancellationToken = default)
            where TViewModel : ICrossViewModel<TParameter>;

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
    Task<bool> Navigate<TViewModel, TParameter>(
        TParameter param, ICrossViewModel source, ICrossBundle? presentationBundle = null,
        CancellationToken cancellationToken = default) where TViewModel : ICrossViewModel<TParameter>
        where TParameter : notnull;

    Task<TResult> Navigate<TViewModel, TResult>(ICrossBundle? presentationBundle = null, CancellationToken cancellationToken = default(CancellationToken))
        where TViewModel : ICrossViewModelResult<TResult>;

    Task<TResult> Navigate<TViewModel, TParameter, TResult>(TParameter param, ICrossBundle? presentationBundle = null, CancellationToken cancellationToken = default(CancellationToken))
        where TViewModel : ICrossViewModel<TParameter, TResult>;

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
    Task<bool> Navigate<TViewModel>(ICrossViewModel source,
        ICrossBundle? presentationBundle = null, CancellationToken cancellationToken = default)
        where TViewModel : ICrossViewModel;
}
