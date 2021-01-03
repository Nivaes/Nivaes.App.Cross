// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross
{
    using System;
    using Autofac;
    using Nivaes.App.Cross.IoC;

    [Obsolete("Sustituir por Autofac")]
    public static class Mvx
    {
        /// <summary>
        /// Returns a singleton instance of the default IoC Provider. If possible use dependency injection instead.
        /// </summary>
        [Obsolete("Sustituir por Autofac")]
        public static IIoCProvider IoCProvider => Nivaes.App.Cross.IoC.IoCProvider.Provider;
    }
}
