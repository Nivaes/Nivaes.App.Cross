// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross.Commands
{
    using System;
    using System.Threading.Tasks;
    using Nivaes.App.Cross.Navigation;
    using Nivaes.App.Cross.ViewModels;

    public class MvxAsyncNavigationCommand<TViewModel>
        : MvxCommandAsync
          where TViewModel : IMvxViewModel
    {
        private static IMvxNavigationService? mNavigationService;

        private static IMvxNavigationService NavigationService => mNavigationService ??= Mvx.IoCProvider.Resolve<IMvxNavigationService>();

        public MvxAsyncNavigationCommand(Func<bool>? canExecute = null, bool allowConcurrentExecutions = false)
           : base(Navigate, canExecute, allowConcurrentExecutions)
        {
        }

        private static ValueTask<bool> Navigate()
        {
            return NavigationService.Navigate<TViewModel>();
        }

        protected override bool CanExecuteImpl(object? parameter)
        {
            return true;
        }
    }
}
