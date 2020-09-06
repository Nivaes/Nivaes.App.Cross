// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.Navigation.EventArguments
{
    using System.ComponentModel;
    using System.Threading;

    public class MvxCancelEventArgs
        : CancelEventArgs
    {
        public MvxCancelEventArgs(CancellationToken? cancellationToken = default)
        {
            CancellationToken = cancellationToken;

            CancellationToken?.Register(Canceled);
        }

        protected CancellationToken? CancellationToken { get; }

        protected virtual void Canceled()
        {
            Cancel = true;
        }
    }
}
