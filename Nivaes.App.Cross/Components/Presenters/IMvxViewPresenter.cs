// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross.Presenters
{
    using System;
    using System.Threading.Tasks;
    using Nivaes.App.Cross.ViewModels;

    public interface IMvxViewPresenter
    {
        ValueTask<bool> Show(MvxViewModelRequest request);

        ValueTask<bool> ChangePresentation(MvxPresentationHint hint);

        void AddPresentationHintHandler<THint>(Func<THint, ValueTask<bool>> action)
            where THint : MvxPresentationHint;

        ValueTask<bool> Close(IMvxViewModel toClose);
    }
}
