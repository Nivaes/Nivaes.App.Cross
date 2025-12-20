namespace MvvmCross.ViewModels
{
    using System.Diagnostics.CodeAnalysis;
    using MvvmCross.Exceptions;
    using MvvmCross.Navigation.EventArguments;
    using Nivaes.App.Cross;

    /// <inheritdoc cref="IMvxViewModelLocator"/>
    public class MvxDefaultViewModelLocator
        : IMvxViewModelLocator
    {
        public virtual ICrossViewModel Load(
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type viewModelType,
            IMvxBundle? parameterValues,
            IMvxBundle? savedState,
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

        public virtual IMvxViewModel<TParameter> Load<TParameter>(
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type viewModelType,
            TParameter param,
            IMvxBundle? parameterValues,
            IMvxBundle? savedState,
            IMvxNavigateEventArgs? navigationArgs = null)
        {
            if (viewModelType == null)
                throw new ArgumentNullException(nameof(viewModelType));

            IMvxViewModel<TParameter> viewModel;
            try
            {
                viewModel = (IMvxViewModel<TParameter>)Mvx.IoCProvider.IoCConstruct(viewModelType);
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
            IMvxBundle? parameterValues,
            IMvxBundle? savedState,
            IMvxNavigateEventArgs? navigationArgs = null)
        {
            RunViewModelLifecycle(viewModel, parameterValues, savedState, navigationArgs);

            return viewModel;
        }

        public virtual IMvxViewModel<TParameter> Reload<TParameter>(
            IMvxViewModel<TParameter> viewModel,
            TParameter param,
            IMvxBundle? parameterValues,
            IMvxBundle? savedState,
            IMvxNavigateEventArgs? navigationArgs = null)
        {
            RunViewModelLifecycle(viewModel, param, parameterValues, savedState, navigationArgs);

            return viewModel;
        }

        protected virtual void CallCustomInitMethods(ICrossViewModel viewModel, IMvxBundle? parameterValues)
        {
            viewModel.CallBundleMethods("Init", parameterValues);
        }

        protected virtual void CallReloadStateMethods(ICrossViewModel viewModel, IMvxBundle? savedState)
        {
            viewModel.CallBundleMethods("ReloadState", savedState);
        }

        protected void RunViewModelLifecycle(
            ICrossViewModel viewModel,
            IMvxBundle? parameterValues,
            IMvxBundle? savedState,
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
            IMvxViewModel<TParameter> viewModel,
            TParameter param,
            IMvxBundle? parameterValues,
            IMvxBundle? savedState,
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
