// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross.tvOS
{
    using MvvmCross.Core;
    using MvvmCross.Platforms.Tvos.Presenters;
    using UIKit;

    public interface IMvxTvosSetup
        : IMvxSetup
    {
        void PlatformInitialize(IMvxApplicationDelegate applicationDelegate, UIWindow window);
        void PlatformInitialize(IMvxApplicationDelegate applicationDelegate, IMvxTvosViewPresenter presenter);
    }
}
