// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.Presenters
{
    using System;
    using System.Threading.Tasks;
    using MvvmCross.ViewModels;

    public interface IMvxViewPresenter
    {
        Task<bool> Show(MvxViewModelRequest request);

        Task<bool> ChangePresentation(MvxPresentationHint hint);

        void AddPresentationHintHandler<THint>(Func<THint, Task<bool>> action)
            where THint : MvxPresentationHint;

        Task<bool> Close(IMvxViewModel toClose);
    }
}
