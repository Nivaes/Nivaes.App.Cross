// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Threading.Tasks;
    using MvvmCross.Plugin;
    using static MvvmCross.Core.MvxSetup;

    public interface IMvxSetup
    {
        MvxSetupState State { get; }

        Task StartSetupInitialization();
        //Task InitializePrimary();
        //Task InitializeSecondary();

        [Obsolete("Not user reflector")]
        IEnumerable<Assembly> GetViewAssemblies();
        [Obsolete("Not user reflector")]
        IEnumerable<Assembly> GetViewModelAssemblies();
        [Obsolete("Not user reflector")]
        IEnumerable<Assembly> GetPluginAssemblies();

        IEnumerable<Type> CreatableTypes();
        IEnumerable<Type> CreatableTypes(Assembly assembly);

        void LoadPlugins(IMvxPluginManager pluginManager);

        event EventHandler<MvxSetupStateEventArgs> StateChanged;
    }
}
