// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross.ViewModels
{
    using System.Threading.Tasks;
    using MvvmCross.Plugin;

    public interface ICrossApplication
        : IMvxViewModelLocatorCollection
    {
        void LoadPlugins(IMvxPluginManager pluginManager);

        ValueTask Initialize();

        ValueTask Startup();

        void Reset();
    }

    public interface ICrossApplication<THint>
        : ICrossApplication
    {
        THint Startup(THint hint);
    }
}
