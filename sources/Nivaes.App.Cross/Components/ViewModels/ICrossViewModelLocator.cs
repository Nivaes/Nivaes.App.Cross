using System.Diagnostics.CodeAnalysis;

namespace Nivaes.App.Cross;

/// <summary>
/// ViewModelLocator helps locating and running start lifecycle of a ViewModel
/// </summary>
[Obsolete]
public interface ICrossViewModelLocator
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
        ICrossBundle? parameterValues,
        ICrossBundle? savedState,
        ICrossNavigateEventArgs? navigationArgs = null);

    /// <summary>
    /// Load ViewModel with parameters
    /// </summary>
    /// <typeparam name="TParameter">Type of parameter to pass into <see cref="ICrossViewModel{TParameter}.Prepare(TParameter)"/> method</typeparam>
    /// <param name="viewModelType"><see cref="Type"/> of ViewModel to load</param>
    /// <param name="param">Parameters to pass into <see cref="ICrossViewModel{TParameter}.Prepare(TParameter)"/> method</param>
    /// <param name="parameterValues">Parameter values to pass into Init methods of ViewModel</param>
    /// <param name="savedState">Saved state to pass into RestoreState methods of ViewModel</param>
    /// <param name="navigationArgs">(Optional) Extra navigation arguments</param>
    /// <returns>Returns a ViewModel</returns>
    ICrossViewModel<TParameter> Load<TParameter>(
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type viewModelType,
        TParameter param,
        ICrossBundle? parameterValues,
        ICrossBundle? savedState,
        ICrossNavigateEventArgs? navigationArgs = null)
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
        ICrossBundle? parameterValues,
        ICrossBundle? savedState,
        ICrossNavigateEventArgs? navigationArgs = null);

    /// <summary>
    /// Reload ViewModel, runs start lifecycle in ViewModel.
    /// </summary>
    /// <typeparam name="TParameter">Type of parameter to pass into <see cref="ICrossViewModel{TParameter}.Prepare(TParameter)"/> method</typeparam>
    /// <param name="viewModel"><see cref="ICrossViewModel"/> to reload</param>
    /// <param name="param">Parameters to pass into <see cref="ICrossViewModel{TParameter}.Prepare(TParameter)"/> method</param>
    /// <param name="parameterValues">Parameter values to pass into Init methods of ViewModel</param>
    /// <param name="savedState">Saved state to pass into RestoreState methods of ViewModel</param>
    /// <param name="navigationArgs">(Optional) Extra navigation arguments</param>
    /// <returns>Returns reloaded ViewModel</returns>
    ICrossViewModel<TParameter> Reload<TParameter>(
        ICrossViewModel<TParameter> viewModel,
        TParameter param,
        ICrossBundle? parameterValues,
        ICrossBundle? savedState,
        ICrossNavigateEventArgs? navigationArgs = null);
}