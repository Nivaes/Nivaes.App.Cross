namespace MvvmCross.ViewModels
{
    using System.Diagnostics.CodeAnalysis;
    using MvvmCross.Navigation.EventArguments;
    using Nivaes.App.Cross;

    /// <summary>
    /// ViewModelLocator helps locating and running start lifecycle of a ViewModel
    /// </summary>
    public interface IMvxViewModelLocator
    {
        /// <summary>
        /// Load ViewModel
        /// </summary>
        /// <param name="viewModelType"><see cref="Type"/> of ViewModel to load</param>
        /// <param name="parameterValues">Parameter values to pass into Init methods of ViewModel</param>
        /// <param name="savedState">Saved state to pass into RestoreState methods of ViewModel</param>
        /// <param name="navigationArgs">(Optional) Extra navigation arguments</param>
        /// <returns>Returns a ViewModel</returns>
        ICrossViewModel Load(
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type viewModelType,
            IMvxBundle? parameterValues,
            IMvxBundle? savedState,
            IMvxNavigateEventArgs? navigationArgs = null);

        /// <summary>
        /// Load ViewModel with parameters
        /// </summary>
        /// <typeparam name="TParameter">Type of parameter to pass into <see cref="IMvxViewModel{TParameter}.Prepare(TParameter)"/> method</typeparam>
        /// <param name="viewModelType"><see cref="Type"/> of ViewModel to load</param>
        /// <param name="param">Parameters to pass into <see cref="IMvxViewModel{TParameter}.Prepare(TParameter)"/> method</param>
        /// <param name="parameterValues">Parameter values to pass into Init methods of ViewModel</param>
        /// <param name="savedState">Saved state to pass into RestoreState methods of ViewModel</param>
        /// <param name="navigationArgs">(Optional) Extra navigation arguments</param>
        /// <returns>Returns a ViewModel</returns>
        IMvxViewModel<TParameter> Load<TParameter>(
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type viewModelType,
            TParameter param,
            IMvxBundle? parameterValues,
            IMvxBundle? savedState,
            IMvxNavigateEventArgs? navigationArgs = null)
            where TParameter : notnull;

        /// <summary>
        /// Reload ViewModel, runs start lifecycle in ViewModel.
        /// </summary>
        /// <param name="viewModel"><see cref="ICrossViewModel"/> to reload</param>
        /// <param name="parameterValues">Parameter values to pass into Init methods of ViewModel</param>
        /// <param name="savedState">Saved state to pass into RestoreState methods of ViewModel</param>
        /// <param name="navigationArgs">(Optional) Extra navigation arguments</param>
        /// <returns>Returns reloaded ViewModel</returns>
        ICrossViewModel Reload(
            ICrossViewModel viewModel,
            IMvxBundle? parameterValues,
            IMvxBundle? savedState,
            IMvxNavigateEventArgs? navigationArgs = null);

        /// <summary>
        /// Reload ViewModel, runs start lifecycle in ViewModel.
        /// </summary>
        /// <typeparam name="TParameter">Type of parameter to pass into <see cref="IMvxViewModel{TParameter}.Prepare(TParameter)"/> method</typeparam>
        /// <param name="viewModel"><see cref="ICrossViewModel"/> to reload</param>
        /// <param name="param">Parameters to pass into <see cref="IMvxViewModel{TParameter}.Prepare(TParameter)"/> method</param>
        /// <param name="parameterValues">Parameter values to pass into Init methods of ViewModel</param>
        /// <param name="savedState">Saved state to pass into RestoreState methods of ViewModel</param>
        /// <param name="navigationArgs">(Optional) Extra navigation arguments</param>
        /// <returns>Returns reloaded ViewModel</returns>
        IMvxViewModel<TParameter> Reload<TParameter>(
            IMvxViewModel<TParameter> viewModel,
            TParameter param,
            IMvxBundle? parameterValues,
            IMvxBundle? savedState,
            IMvxNavigateEventArgs? navigationArgs = null);
    }
}