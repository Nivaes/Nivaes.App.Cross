// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross.Sample
{
    using Nivaes.App.Cross.Commands;
    using Nivaes.App.Cross.Logging;
    using Nivaes.App.Cross.Navigation;
    using Nivaes.App.Cross.ViewModels;

    public class MixedNavResultDetailViewModel
        : MvxNavigationViewModelResult<DetailResultResult>, IMvxViewModelResult<DetailResultResult>
    {
        public MixedNavResultDetailViewModel(/*IMvxLogProvider logProvider,*/ IMvxNavigationService navigationService)
            : base(/*logProvider,*/ navigationService)
        {
            CloseViewModelCommand = new MvxCommandAsync(async () => await NavigationService.Close(this, DetailResultResult.Build()).ConfigureAwait(true));
        }


        public IMvxCommandAsync CloseViewModelCommand { get; private set; }
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
