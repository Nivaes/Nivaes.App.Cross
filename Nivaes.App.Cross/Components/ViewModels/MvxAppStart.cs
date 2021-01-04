// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross.ViewModels
{
    using System.Threading;
    using System.Threading.Tasks;
    using MvvmCross.Exceptions;
    using Nivaes.App.Cross.Logging;
    using Nivaes.App.Cross.Navigation;

    public abstract class MvxAppStart
        : IMvxAppStart
    {
        protected IMvxNavigationService NavigationService { get; }
        protected ICrossApplication Application { get; }

        private int startHasCommenced;

        public MvxAppStart(ICrossApplication application, IMvxNavigationService navigationService)
        {
            Application = application;
            NavigationService = navigationService;
        }

        public async ValueTask Start(object? hint = null)
        {
            // Check whether Start has commenced, and return if it has
            if (Interlocked.CompareExchange(ref startHasCommenced, 1, 0) == 1)
                return;

            var applicationHint = await ApplicationStartup(hint).ConfigureAwait(false);
            //if (applicationHint != null)
            //{
            //    MvxLog.Instance?.Trace("Hint ignored in default MvxAppStart");
            //}

            await NavigateToFirstViewModel(applicationHint).ConfigureAwait(false);
        }

        protected abstract ValueTask<bool> NavigateToFirstViewModel(object? hint = null);

        protected virtual async Task<object?> ApplicationStartup(object? hint = null)
        {
            await Application.Startup().ConfigureAwait(false);
            return hint;
        }

        public virtual bool IsStarted => startHasCommenced != 0;

        public virtual void ResetStart()
        {
            Reset();
            Interlocked.Exchange(ref startHasCommenced, 0);
        }

        protected virtual void Reset()
        {
            Application.Reset();
        }
    }

    public class MvxAppStart<TViewModel> : MvxAppStart
        where TViewModel : IMvxViewModel
    {
        public MvxAppStart(ICrossApplication application, IMvxNavigationService navigationService) : base(application, navigationService)
        {
        }

        protected override ValueTask<bool> NavigateToFirstViewModel(object? hint = null)
        {
            try
            {
                return NavigationService.Navigate<TViewModel>();
            }
            catch (System.Exception ex)
            {
                throw new MvxException($"Problem navigating to ViewModel {typeof(TViewModel).Name}", ex);
            }
        }
    }

    public class MvxAppStart<TViewModel, TParameter>
        : MvxAppStart<TViewModel> where TViewModel : IMvxViewModel<TParameter>
    {
        public MvxAppStart(ICrossApplication application, IMvxNavigationService navigationService) : base(application, navigationService)
        {
        }

        protected override async Task<object?> ApplicationStartup(object? hint = null)
        {
            var applicationHint = await base.ApplicationStartup(hint).ConfigureAwait(false);
            if (applicationHint is TParameter parameter && Application is ICrossApplication<TParameter> typedApplication)
            {
                return typedApplication.Startup(parameter);
            }
            else
            {
                return applicationHint;
            }
        }

        protected override ValueTask<bool> NavigateToFirstViewModel(object? hint = null)
        {
            try
            {
                if (hint is TParameter parameter)
                {
                    return NavigationService.Navigate<TViewModel, TParameter>(parameter);
                }
                else
                {
                    //MvxLog.Instance?.Trace($"Hint is not matching type of {nameof(TParameter)}. Doing navigation without typed parameter instead.");
                    return base.NavigateToFirstViewModel(hint);
                }
            }
            catch (System.Exception ex)
            {
                throw new MvxException($"Problem navigating to ViewModel {typeof(TViewModel).Name}", ex);
            }
        }
    }
}
