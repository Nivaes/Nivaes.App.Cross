// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross.Sample
{
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross;
    using Nivaes.App.Cross.Logging;
    using Nivaes.App.Cross.Navigation;

    public class FragmentCloseViewModel
        : MvxNavigationViewModel
    {
        private static int mCounter;

        public FragmentCloseViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService)
            : base(logProvider, navigationService)
        {
            ForwardCommand = new MvxAsyncCommand(async () => await base.NavigationService.Navigate<FragmentCloseViewModel>().ConfigureAwait(true));
            CloseCommand = new MvxAsyncCommand(async () => await base.NavigationService.Close(this).ConfigureAwait(true));

            Description = $"View number {mCounter++}";
        }

        private string mDescription = string.Empty;

        public string Description
        {
            get => mDescription;
            set => SetProperty(ref mDescription, value);
        }

        public IMvxAsyncCommand ForwardCommand { get; }

        public IMvxAsyncCommand CloseCommand { get; }
    }
}
