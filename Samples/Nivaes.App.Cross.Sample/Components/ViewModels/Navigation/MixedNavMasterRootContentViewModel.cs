// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross.Sample
{
    using Nivaes.App.Cross.Commands;
    using Nivaes.App.Cross.Logging;
    using Nivaes.App.Cross.Navigation;
    using Nivaes.App.Cross.ViewModels;

    public class MixedNavMasterRootContentViewModel
        : MvxNavigationViewModel
    {
        public MixedNavMasterRootContentViewModel(/*IMvxLogProvider logProvider,*/ IMvxNavigationService navigationService)
            : base(/*logProvider,*/ navigationService)
        {
            ShowModalCommand = new MvxCommandAsync(() => NavigationService.Navigate<ModalNavViewModel>());
            ShowChildCommand = new MvxCommandAsync(() => NavigationService.Navigate<ChildViewModel, SampleModel>(new SampleModel { Message = "Hey", Value = 1.23m }));
        }

        public IMvxCommandAsync ShowModalCommand { get; }
        public IMvxCommandAsync ShowChildCommand { get; }
    }
}
