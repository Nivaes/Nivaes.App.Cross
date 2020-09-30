// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross.Presenters
{
    using MvvmCross.ViewModels;

    public interface IMvxOverridePresentationAttribute
    {
        MvxBasePresentationAttribute PresentationAttribute(MvxViewModelRequest request);
    }
}
