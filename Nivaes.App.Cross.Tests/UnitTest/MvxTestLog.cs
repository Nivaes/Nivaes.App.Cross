// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.Tests
{
    using Nivaes.App.Cross.Logging;
    using Nivaes.App.Cross.Navigation;

    internal static class MvxTestLog
    {
        internal static IMvxLog Instance { get; } = Mvx.IoCProvider.Resolve<IMvxLogProvider>().GetLogFor("MvxTest");
    }
}
