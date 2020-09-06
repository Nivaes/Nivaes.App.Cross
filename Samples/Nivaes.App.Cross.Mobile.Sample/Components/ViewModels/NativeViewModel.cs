// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Mobile.Sample
{
    using System.Threading.Tasks;
    using MvvmCross.Commands;
    using MvvmCross.Navigation;
    using MvvmCross.ViewModels;

    public class NativeViewModel
        : MvxViewModel
    {
        private static int mCounter;

        public NativeViewModel(IMvxNavigationService navigationService)
        {
            ForwardCommand = new MvxAsyncCommand(
                async () => await navigationService.Navigate<NativeViewModel>().ConfigureAwait(false));
            CloseCommand = new MvxAsyncCommand(
                async () => await navigationService.Close(this).ConfigureAwait(false));

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
