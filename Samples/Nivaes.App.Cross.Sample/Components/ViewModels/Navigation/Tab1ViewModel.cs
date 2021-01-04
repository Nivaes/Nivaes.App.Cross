// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross.Sample
{
    using System.Threading.Tasks;
    using Nivaes.App.Cross.Commands;
    using Nivaes.App.Cross.Logging;
    using Nivaes.App.Cross.Navigation;
    using Nivaes.App.Cross.Presenters;
    using Nivaes.App.Cross.ViewModels;

    public class Tab1ViewModel
        : MvxNavigationViewModel<string>
    {
        public Tab1ViewModel(/*IMvxLogProvider logProvider,*/ IMvxNavigationService navigationService)
            : base(/*logProvider,*/ navigationService)
        {
            OpenChildCommand = new MvxCommandAsync(async () => await NavigationService.Navigate<ChildViewModel>().ConfigureAwait(true));

            OpenModalCommand = new MvxCommandAsync(async () => await NavigationService.Navigate<ModalViewModel>().ConfigureAwait(true));

            OpenNavModalCommand = new MvxCommandAsync(async () => await NavigationService.Navigate<ModalNavViewModel>().ConfigureAwait(true));

            CloseCommand = new MvxCommandAsync(async () => await NavigationService.Close(this).ConfigureAwait(true));

            OpenTab2Command = new MvxCommandAsync(async () => await NavigationService.ChangePresentation(new MvxPagePresentationHint(typeof(Tab2ViewModel))).ConfigureAwait(true));
        }

        public override ValueTask Initialize()
        {
            return new ValueTask(Task.Delay(3000));
        }

        private string para;
        public override ValueTask Prepare(string parameter)
        {
            para = parameter;

            return new ValueTask();
        }

        public override ValueTask ViewAppeared()
        {
            return base.ViewAppeared();
        }

        public IMvxCommandAsync OpenChildCommand { get; private set; }

        public IMvxCommandAsync OpenModalCommand { get; private set; }

        public IMvxCommandAsync OpenNavModalCommand { get; private set; }

        public IMvxCommandAsync OpenTab2Command { get; private set; }

        public IMvxCommandAsync CloseCommand { get; private set; }
    }
}
