using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;

namespace Nivaes.App.Cross;

/// <inheritdoc cref="ICrossViewModelLocator"/>
internal sealed class CrossViewModelLocator
    //: ICrossViewModelLocator
{
    private IServiceProvider _serviceProvider;

    public CrossViewModelLocator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public ICrossViewModel Load(
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
        catch (Exception ex)
        {
            throw new AppException(ex, $"Problem creating viewModel of type {viewModelType.Name}");
        }

        if (viewModel == null)
        {
            throw new AppException($"Not resolve viewModel of type {viewModelType.Name}.");
        }

        RunViewModelLifecycle(viewModel, parameterValues, savedState, navigationArgs);

        return viewModel;
    }

    public ICrossViewModel<TParameter> Load<TParameter>(
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
        catch (Exception ex)
        {
            throw new AppException(ex, $"Problem creating viewModel of type {viewModelType.Name}");
        }

        if (viewModel == null)
        {
            throw new AppException($"Not resolve viewModel of type {viewModelType.Name}.");
        }

        RunViewModelLifecycle(viewModel, param, parameterValues, savedState, navigationArgs);

        return viewModel;
    }

    public ICrossViewModel Reload(
        ICrossViewModel viewModel,
        ICrossBundle? parameterValues,
        ICrossBundle? savedState,
        ICrossNavigateEventArgs? navigationArgs = null)
    {
        RunViewModelLifecycle(viewModel, parameterValues, savedState, navigationArgs);

        return viewModel;
    }

    public ICrossViewModel<TParameter> Reload<TParameter>(
        ICrossViewModel<TParameter> viewModel,
        TParameter param,
        ICrossBundle? parameterValues,
        ICrossBundle? savedState,
        ICrossNavigateEventArgs? navigationArgs = null)
    {
        RunViewModelLifecycle(viewModel, param, parameterValues, savedState, navigationArgs);

        return viewModel;
    }

    private void CallCustomInitMethods(ICrossViewModel viewModel, ICrossBundle? parameterValues)
    {
        viewModel.CallBundleMethods("Init", parameterValues);
    }

    private void CallReloadStateMethods(ICrossViewModel viewModel, ICrossBundle? savedState)
    {
        viewModel.CallBundleMethods("ReloadState", savedState);
    }

    private void RunViewModelLifecycle(
        ICrossViewModel viewModel,
        ICrossBundle? parameterValues,
        ICrossBundle? savedState,
        ICrossNavigateEventArgs? navigationArgs)
    {
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
        catch (Exception ex)
        {
            throw new AppException(ex, "Problem running viewModel lifecycle of type {0}", viewModel.GetType().Name);
        }
    }

    private void RunViewModelLifecycle<TParameter>(
        ICrossViewModel<TParameter> viewModel,
        TParameter param,
        ICrossBundle? parameterValues,
        ICrossBundle? savedState,
        ICrossNavigateEventArgs? navigationArgs)
    {
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
        catch (Exception ex)
        {
            throw new AppException(ex, "Problem running viewModel lifecycle of type {0}", viewModel.GetType().Name);
        }
    }
}
