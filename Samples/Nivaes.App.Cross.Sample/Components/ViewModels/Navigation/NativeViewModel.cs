// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross.Sample
{
    using Nivaes.App.Cross.Commands;
    using Nivaes.App.Cross.Navigation;
    using Nivaes.App.Cross.ViewModels;

    public class NativeViewModel
        : MvxViewModel
    {
        private static int mCounter;

        public NativeViewModel(IMvxNavigationService navigationService)
        {
            ForwardCommand = new MvxCommandAsync(
                async () => await navigationService.Navigate<NativeViewModel>().ConfigureAwait(false));
            CloseCommand = new MvxCommandAsync(
                async () => await navigationService.Close(this).ConfigureAwait(false));

            Description = $"View number {mCounter++}";
        }

        private string mDescription = string.Empty;

        public string Description
        {
            get => mDescription;
            set => SetProperty(ref mDescription, value);
        }

        public IMvxCommandAsync ForwardCommand { get; }

        public IMvxCommandAsync CloseCommand { get; }

    }
}
