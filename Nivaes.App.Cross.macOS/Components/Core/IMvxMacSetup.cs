// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross.macOS
{
    using AppKit;
    using MvvmCross.Platforms.Mac.Presenters;
    using Nivaes.App.Cross;

    public interface IMvxMacSetup
        : IMvxSetup
    {
        void PlatformInitialize(IMvxApplicationDelegate applicationDelegate);
        void PlatformInitialize(IMvxApplicationDelegate applicationDelegate, NSWindow window);
        void PlatformInitialize(IMvxApplicationDelegate applicationDelegate, IMvxMacViewPresenter presenter);
    }
}
