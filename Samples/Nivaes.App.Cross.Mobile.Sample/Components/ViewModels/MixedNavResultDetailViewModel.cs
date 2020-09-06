// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Mobile.Sample
{
    using System;
    using System.Threading.Tasks;
    using MvvmCross.Commands;
    using MvvmCross.Logging;
    using MvvmCross.Navigation;
    using MvvmCross.ViewModels;

    public class MixedNavResultDetailViewModel
        : MvxNavigationViewModelResult<DetailResultResult>, IMvxViewModelResult<DetailResultResult>
    {
        public MixedNavResultDetailViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService) : base(logProvider, navigationService)
        {
            CloseViewModelCommand = new MvxAsyncCommand(async () => await NavigationService.Close(this, DetailResultResult.Build()).ConfigureAwait(true));
        }


        public IMvxAsyncCommand CloseViewModelCommand { get; private set; }
    }

    public class DetailResultParams
    {
    }

    public class DetailResultResult
    {
        public static DetailResultResult Build()
        {
            return new DetailResultResult();
        }
    }
}
