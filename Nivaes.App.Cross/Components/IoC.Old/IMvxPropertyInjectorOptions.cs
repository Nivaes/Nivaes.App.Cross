﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;

namespace MvvmCross.IoC
{
    [Obsolete("Sustituir por Autofac")]
    public interface IMvxPropertyInjectorOptions
    {
        MvxPropertyInjection InjectIntoProperties { get; }
        bool ThrowIfPropertyInjectionFails { get; }
    }
}