// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.Core
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Threading.Tasks;
    using MvvmCross.Logging;
    using MvvmCross.Plugin;
    using static MvvmCross.Core.MvxSetup;

    public interface IMvxSetup
    {
        MvxSetupState State { get; }

        Task InitializePrimary();
        Task InitializeSecondary();

        IEnumerable<Assembly> GetViewAssemblies();
        IEnumerable<Assembly> GetViewModelAssemblies();
        IEnumerable<Assembly> GetPluginAssemblies();

        IEnumerable<Type> CreatableTypes();
        IEnumerable<Type> CreatableTypes(Assembly assembly);

        void LoadPlugins(IMvxPluginManager pluginManager);

        MvxLogProviderType GetDefaultLogProviderType();

        event EventHandler<MvxSetupStateEventArgs> StateChanged;
    }
}
