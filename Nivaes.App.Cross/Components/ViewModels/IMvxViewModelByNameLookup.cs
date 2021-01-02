// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross.ViewModels
{
    using System;

    public interface IMvxViewModelByNameLookup
    {
        bool TryLookupByName(string name, out Type? viewModelType);

        bool TryLookupByFullName(string name, out Type? viewModelType);
    }
}
