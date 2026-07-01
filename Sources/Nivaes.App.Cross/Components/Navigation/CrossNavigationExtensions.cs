using System.Diagnostics.CodeAnalysis;

namespace Nivaes.App.Cross;

public static class CrossNavigationExtensions
{
    extension(ICrossNavigationService navigationService)
    {
        ///// <summary>
        ///// Verifies if the provided Uri can be routed to a ViewModel request.
        ///// </summary>
        ///// <param name="path">URI to route</param>
        ///// <returns>True if the uri can be routed or false if it cannot.</returns>
        //public Task<bool> CanNavigate(Uri path)
        //{
        //    return navigationService.CanNavigate(path.ToString());
        //}

        ///// <summary>
        ///// Translates the provided Uri to a ViewModel request and dispatches it.
        ///// </summary>
        ///// <param name="path">URI to route</param>
        ///// <param name="presentationBundle"></param>
        ///// <param name="cancellationToken"></param>
        ///// <returns>A task to await upon</returns>
        //[RequiresUnreferencedCode("This method uses reflection which may not be preserved during trimming")]
        //public Task Navigate(Uri path, ICrossBundle? presentationBundle = null, CancellationToken cancellationToken = default)
        //{
        //    return navigationService.Navigate(path.ToString(), presentationBundle, cancellationToken);
        //}

        //[RequiresUnreferencedCode("This method uses reflection which may not be preserved during trimming")]
        //public Task Navigate<TParameter>(Uri path, TParameter param, ICrossBundle? presentationBundle = null, CancellationToken cancellationToken = default)
        //{
        //    return navigationService.Navigate(path.ToString(), param, presentationBundle, cancellationToken);
        //}

        /// <summary>
        /// Navigate from a Result Awaiting ViewModel to a Result Setting ViewModel determined by its type
        /// </summary>
        /// <param name="fromViewModel">Result Awaiting ViewModel</param>
        /// <param name="resultViewModelManager">Result ViewModel Manager</param>
        /// <param name="presentationBundle">(optional) presentation bundle</param>
        /// <param name="cancellationToken">(optional) CancellationToken to cancel the navigation</param>
        /// <typeparam name="TViewModel">Type of <see cref="ICrossResultSettingViewModel{TResult}"/></typeparam>
        /// <typeparam name="TResult">Result awaited by Result Awaiting ViewModel and set by Result Setting ViewModel</typeparam>
        /// <returns>Boolean indicating successful navigation</returns>
        public async Task<bool> NavigateRegisteringToResult<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TViewModel, TResult>(
            ICrossResultAwaitingViewModel<TResult> fromViewModel,
            ICrossResultViewModelManager resultViewModelManager,
            ICrossBundle? presentationBundle = null,
            CancellationToken cancellationToken = default)
            where TViewModel : ICrossResultSettingViewModel<TResult>, ICrossViewModel
        {
            bool navigated = await navigationService.Navigate<TViewModel>(presentationBundle, cancellationToken);
            if (navigated)
                fromViewModel.RegisterToResult(resultViewModelManager);
            return navigated;
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
        public async Task<bool> NavigateRegisteringToResult<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TViewModel, TParameter, TResult>(
            ICrossResultAwaitingViewModel<TResult> fromViewModel,
            ICrossResultViewModelManager resultViewModelManager,
            TParameter parameter,
            ICrossBundle? presentationBundle = null,
            CancellationToken cancellationToken = default)
            where TViewModel : ICrossResultSettingViewModel<TResult>, ICrossViewModel<TParameter>
        {
            bool navigated = await navigationService.Navigate<TViewModel, TParameter>(parameter, presentationBundle, cancellationToken);
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
            bool closed = await navigationService.Close(viewModel, cancellationToken);
            if (closed)
                viewModel.SetResult(result);
            return closed;
        }
    }
}