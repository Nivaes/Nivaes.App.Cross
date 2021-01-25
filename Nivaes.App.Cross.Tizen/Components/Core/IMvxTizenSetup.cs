// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Tizen.Applications;

namespace MvvmCross.Platforms.Tizen.Core
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using MvvmCross.Core;
    using Nivaes.App.Cross;

    public interface IMvxTizenSetup
        : IMvxSetup
    {
        void PlatformInitialize(CoreApplication coreApplication);
    }
}
