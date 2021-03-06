﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross.ViewModels
{
    using System.Threading.Tasks;
    using Nivaes.App.Cross.Logging;
    using Nivaes.App.Cross.Navigation;

    public abstract class MvxNavigationViewModel
        : MvxViewModel
    {
        //private IMvxLog? mLog;

        protected MvxNavigationViewModel(/*IMvxLogProvider logProvider,*/ IMvxNavigationService navigationService)
            : base()
        {
            //LogProvider = logProvider;
            NavigationService = navigationService;
        }

        protected virtual IMvxNavigationService NavigationService { get; }

        //protected virtual IMvxLogProvider LogProvider { get; }

        //protected virtual IMvxLog Log => mLog ??= LogProvider.GetLogFor(GetType());
    }

    public abstract class MvxNavigationViewModel<TParameter>
        : MvxNavigationViewModel, IMvxViewModel<TParameter>
    {
        protected MvxNavigationViewModel(/*IMvxLogProvider logProvider,*/ IMvxNavigationService navigationService)
            : base(/*logProvider,*/ navigationService)
        {
        }

        public abstract ValueTask Prepare(TParameter parameter);
    }

    //TODO: Not possible to name MvxViewModel, name is MvxViewModelResult for now
    public abstract class MvxNavigationViewModelResult<TResult>
        : MvxNavigationViewModel, IMvxViewModelResult<TResult>
    {
        protected MvxNavigationViewModelResult(/*IMvxLogProvider logProvider,*/ IMvxNavigationService navigationService)
            : base(/*logProvider,*/ navigationService)
        {
        }

        public TaskCompletionSource<object>? CloseCompletionSource { get; set; }

        public override ValueTask ViewDestroy(bool viewFinishing = true)
        {
            if (viewFinishing && CloseCompletionSource?.Task.IsCompleted == false && !CloseCompletionSource.Task.IsFaulted)
                CloseCompletionSource?.TrySetCanceled();

            return base.ViewDestroy(viewFinishing);
        }
    }

    public abstract class MvxNavigationViewModel<TParameter, TResult>
        : MvxNavigationViewModelResult<TResult>, IMvxViewModel<TParameter, TResult>
    {
        protected MvxNavigationViewModel(/*IMvxLogProvider logProvider,*/ IMvxNavigationService navigationService)
            : base(/*logProvider,*/ navigationService)
        {
        }

        public abstract ValueTask Prepare(TParameter parameter);
    }
}
