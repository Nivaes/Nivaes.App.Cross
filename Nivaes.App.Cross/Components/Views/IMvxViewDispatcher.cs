// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.Views
{
    using System.Threading.Tasks;
    using MvvmCross.Base;
    using MvvmCross.ViewModels;

    public interface IMvxViewDispatcher
        : IMvxMainThreadDispatcher
    {
        ValueTask<bool> ShowViewModel(MvxViewModelRequest request);

        ValueTask<bool> ChangePresentation(MvxPresentationHint hint);
    }
}
