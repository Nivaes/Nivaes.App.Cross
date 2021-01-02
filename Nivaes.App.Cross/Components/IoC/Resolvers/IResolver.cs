// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross.IoC
{
    using System;

    internal interface IResolver
    {
        object Resolve();

        ResolverType ResolveType { get; }

        void SetGenericTypeParameters(Type[] genericTypeParameters);
    }
}
