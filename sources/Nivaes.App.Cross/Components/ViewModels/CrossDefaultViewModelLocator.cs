namespace Nivaes.App.Cross
{
    using System.Diagnostics.CodeAnalysis;
    using MvvmCross;
    using MvvmCross.Exceptions;
    using MvvmCross.Navigation.EventArguments;
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross;

    /// <inheritdoc cref="ICrossViewModelLocator"/>
    public class CrossDefaultViewModelLocator
        : ICrossViewModelLocator
    {
        public virtual ICrossViewModel Load(
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type viewModelType,
            ICrossBundle? parameterValues,
            ICrossBundle? savedState,
            IMvxNavigateEventArgs? navigationArgs = null)
        {
            if (viewModelType == null)
                throw new ArgumentNullException(nameof(viewModelType));

            ICrossViewModel viewModel;
            try
            {
                viewModel = (ICrossViewModel)Mvx.IoCProvider.IoCConstruct(viewModelType);
            }
            catch (Exception exception)
            {
                throw exception.MvxWrap("Problem creating viewModel of type {0}", viewModelType.Name);
            }

            RunViewModelLifecycle(viewModel, parameterValues, savedState, navigationArgs);

            return viewModel;
        }

        public virtual ICrossViewModel<TParameter> Load<TParameter>(
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type viewModelType,
            TParameter param,
            ICrossBundle? parameterValues,
            ICrossBundle? savedState,
            IMvxNavigateEventArgs? navigationArgs = null)
        {
            if (viewModelType == null)
                throw new ArgumentNullException(nameof(viewModelType));

            ICrossViewModel<TParameter> viewModel;
            try
            {
                viewModel = (ICrossViewModel<TParameter>)Mvx.IoCProvider.IoCConstruct(viewModelType);
            }
            catch (Exception exception)
            {
                throw exception.MvxWrap("Problem creating viewModel of type {0}", viewModelType.Name);
            }

            RunViewModelLifecycle(viewModel, param, parameterValues, savedState, navigationArgs);

            return viewModel;
        }

        public virtual ICrossViewModel Reload(
            ICrossViewModel viewModel,
            ICrossBundle? parameterValues,
            ICrossBundle? savedState,
            IMvxNavigateEventArgs? navigationArgs = null)
        {
            RunViewModelLifecycle(viewModel, parameterValues, savedState, navigationArgs);

            return viewModel;
        }

        public virtual ICrossViewModel<TParameter> Reload<TParameter>(
            ICrossViewModel<TParameter> viewModel,
            TParameter param,
            ICrossBundle? parameterValues,
            ICrossBundle? savedState,
            IMvxNavigateEventArgs? navigationArgs = null)
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
            IMvxNavigateEventArgs? navigationArgs)
        {
            if (viewModel == null)
                throw new ArgumentNullException(nameof(viewModel));

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

                viewModel.InitializeTask = MvxNotifyTask.Create(() => viewModel.Initialize());
            }
            catch (Exception exception)
            {
                throw exception.MvxWrap("Problem running viewModel lifecycle of type {0}", viewModel.GetType().Name);
            }
        }

        protected void RunViewModelLifecycle<TParameter>(
            ICrossViewModel<TParameter> viewModel,
            TParameter param,
            ICrossBundle? parameterValues,
            ICrossBundle? savedState,
            IMvxNavigateEventArgs? navigationArgs)
        {
            if (viewModel == null)
                throw new ArgumentNullException(nameof(viewModel));

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

                viewModel.InitializeTask = MvxNotifyTask.Create(() => viewModel.Initialize());
            }
            catch (Exception exception)
            {
                throw exception.MvxWrap("Problem running viewModel lifecycle of type {0}", viewModel.GetType().Name);
            }
        }
    }
}
