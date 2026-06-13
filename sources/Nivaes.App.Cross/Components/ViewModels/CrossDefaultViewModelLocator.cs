using Microsoft.Extensions.DependencyInjection;

namespace Nivaes.App.Cross;

/// <inheritdoc cref="ICrossViewModelLocator"/>
public class CrossDefaultViewModelLocator
    : ICrossViewModelLocator
{
    private IServiceProvider _serviceProvider;

    public CrossDefaultViewModelLocator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    // ToDo: ¿Tiene sentido sobrecargar esta clase?
    public virtual ICrossViewModel Load(
        Type viewModelType,
        ICrossBundle? parameterValues,
        ICrossBundle? savedState,
        ICrossNavigateEventArgs? navigationArgs = null)
    {
        ICrossViewModel? viewModel;
        try
        {
            viewModel = (ICrossViewModel?)ActivatorUtilities.CreateInstance(_serviceProvider, viewModelType);
        }
        catch (Exception exception)
        {
            throw exception.Wrap($"Problem creating viewModel of type {viewModelType.Name}");
        }

        if (viewModel == null)
        {
            throw new CrossException($"Not resolve viewModel of type {viewModelType.Name}.");
        }

        RunViewModelLifecycle(viewModel, parameterValues, savedState, navigationArgs);

        return viewModel;
    }

    // ToDo: ¿Tiene sentido sobrecargar esta clase?
    public virtual ICrossViewModel<TParameter> Load<TParameter>(
        Type viewModelType,
        TParameter param,
        ICrossBundle? parameterValues,
        ICrossBundle? savedState,
        ICrossNavigateEventArgs? navigationArgs = null)
        where TParameter : notnull
    {
        ICrossViewModel<TParameter>? viewModel;
        try
        {
            viewModel = (ICrossViewModel<TParameter>?)ActivatorUtilities.CreateInstance(_serviceProvider, viewModelType);
        }
        catch (Exception exception)
        {
            throw exception.Wrap($"Problem creating viewModel of type {viewModelType.Name}");
        }

        if (viewModel == null)
        {
            throw new CrossException($"Not resolve viewModel of type {viewModelType.Name}.");
        }

        RunViewModelLifecycle(viewModel, param, parameterValues, savedState, navigationArgs);

        return viewModel;
    }

    public virtual ICrossViewModel Reload(
        ICrossViewModel viewModel,
        ICrossBundle? parameterValues,
        ICrossBundle? savedState,
        ICrossNavigateEventArgs? navigationArgs = null)
    {
        RunViewModelLifecycle(viewModel, parameterValues, savedState, navigationArgs);

        return viewModel;
    }

    public virtual ICrossViewModel<TParameter> Reload<TParameter>(
        ICrossViewModel<TParameter> viewModel,
        TParameter param,
        ICrossBundle? parameterValues,
        ICrossBundle? savedState,
        ICrossNavigateEventArgs? navigationArgs = null)
    {
        RunViewModelLifecycle(viewModel, param, parameterValues, savedState, navigationArgs);

        return viewModel;
    }

    protected virtual void CallCustomInitMethods(ICrossViewModel viewModel, ICrossBundle? parameterValues)
    {
        viewModel.CallBundleMethods("Init", parameterValues);
    }

    protected virtual void CallReloadStateMethods(ICrossViewModel viewModel, ICrossBundle? savedState)
    {
        viewModel.CallBundleMethods("ReloadState", savedState);
    }

    protected void RunViewModelLifecycle(
        ICrossViewModel viewModel,
        ICrossBundle? parameterValues,
        ICrossBundle? savedState,
        ICrossNavigateEventArgs? navigationArgs)
    {
        ArgumentNullException.ThrowIfNull(viewModel, nameof(viewModel));

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
            viewModel.Start();
            if (navigationArgs?.Cancel == true)
                return;

            viewModel.Prepare();
            if (navigationArgs?.Cancel == true)
                return;

            viewModel.InitializeTask = CrossNotifyTask.Create(() => viewModel.Initialize());
        }
        catch (Exception exception)
        {
            throw exception.Wrap("Problem running viewModel lifecycle of type {0}", viewModel.GetType().Name);
        }
    }

    protected void RunViewModelLifecycle<TParameter>(
        ICrossViewModel<TParameter> viewModel,
        TParameter param,
        ICrossBundle? parameterValues,
        ICrossBundle? savedState,
        ICrossNavigateEventArgs? navigationArgs)
    {
        ArgumentNullException.ThrowIfNull(viewModel, nameof(viewModel));

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
            viewModel.Start();
            if (navigationArgs?.Cancel == true)
                return;

            viewModel.Prepare();
            if (navigationArgs?.Cancel == true)
                return;

            viewModel.Prepare(param);
            if (navigationArgs?.Cancel == true)
                return;

            viewModel.InitializeTask = CrossNotifyTask.Create(() => viewModel.Initialize());
        }
        catch (Exception exception)
        {
            throw exception.Wrap("Problem running viewModel lifecycle of type {0}", viewModel.GetType().Name);
        }
    }
}
