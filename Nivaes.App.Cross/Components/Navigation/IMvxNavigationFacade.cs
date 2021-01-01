// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross.Navigation
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Nivaes.App.Cross.ViewModels;

    public interface IMvxNavigationFacade
    {
        Task<MvxViewModelRequest> BuildViewModelRequest(string url, IDictionary<string, string> currentParameters);
    }
}
