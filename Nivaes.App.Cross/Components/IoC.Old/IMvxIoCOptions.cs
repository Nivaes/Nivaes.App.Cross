// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross.IoC
{

    using System;
    using MvvmCross.IoC;

    [Obsolete("Sustituir por Autofac")]
    public interface IMvxIocOptions
    {
        bool TryToDetectSingletonCircularReferences { get; }
        bool TryToDetectDynamicCircularReferences { get; }
        bool CheckDisposeIfPropertyInjectionFails { get; }
        Type PropertyInjectorType { get; }
        IMvxPropertyInjectorOptions PropertyInjectorOptions { get; }
    }
}
