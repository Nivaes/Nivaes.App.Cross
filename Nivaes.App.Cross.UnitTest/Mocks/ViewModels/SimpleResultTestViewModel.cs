// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross.UnitTest
{
    using System.Threading.Tasks;
    using MvvmCross.ViewModels;

    public class SimpleResultTestViewModel
        : MvxViewModelResult<bool>
    {
        public SimpleResultTestViewModel()
        {
        }

        public override async ValueTask Initialize()
        {
            await base.Initialize().ConfigureAwait(true);

            await Task.Delay(2000).ConfigureAwait(true);
            CloseCompletionSource?.SetResult(true);
        }
    }
}
