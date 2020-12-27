// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross.Navigation
{
    using System.Threading;
    using MvvmCross.ViewModels;

    public class ChangePresentationEventArgs : MvxCancelEventArgs
    {
        public ChangePresentationEventArgs(CancellationToken? cancellationToken = default)
            : base(cancellationToken)
        {
        }

        public ChangePresentationEventArgs(MvxPresentationHint hint, CancellationToken? cancellationToken = default)
            : this(cancellationToken)
        {
            Hint = hint;
        }

        public MvxPresentationHint? Hint { get; set; }

        public bool? Result { get; set; }
    }
}
