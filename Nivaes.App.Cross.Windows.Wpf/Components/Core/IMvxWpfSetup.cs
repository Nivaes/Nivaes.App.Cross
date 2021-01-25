// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross.Wpf
{
    using System.Windows.Controls;
    using System.Windows.Threading;
    using MvvmCross.Platforms.Wpf.Presenters;
    using Nivaes.App.Cross;

    public interface IMvxWpfSetup
        : IMvxSetup
    {
        void PlatformInitialize(Dispatcher uiThreadDispatcher, IMvxWpfViewPresenter presenter);
        void PlatformInitialize(Dispatcher uiThreadDispatcher, ContentControl root);
    }
}
