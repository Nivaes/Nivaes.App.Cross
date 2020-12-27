// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross.Sample
{
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross;
    using Nivaes.App.Cross.Logging;
    using Nivaes.App.Cross.Navigation;
    using Nivaes.App.Cross.Presenters;

    public class NestedChildViewModel
        : MvxNavigationViewModel
    {
        public NestedChildViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService) : base(logProvider, navigationService)
        {
            CloseCommand = new MvxAsyncCommand(async () => await NavigationService.Close(this).ConfigureAwait(true));
            PopToChildCommand = new MvxAsyncCommand(() => NavigationService.ChangePresentation(new MvxPopPresentationHint(typeof(ChildViewModel))));
            PopToRootCommand = new MvxAsyncCommand(() => NavigationService.ChangePresentation(new MvxPopToRootPresentationHint()));
            RemoveCommand = new MvxAsyncCommand(() => NavigationService.ChangePresentation(new MvxRemovePresentationHint(typeof(SecondChildViewModel))));
        }

        public IMvxAsyncCommand CloseCommand { get; private set; }

        public IMvxCommand PopToChildCommand { get; private set; }

        public IMvxCommand PopToRootCommand { get; private set; }

        public IMvxCommand RemoveCommand { get; private set; }
    }
}
