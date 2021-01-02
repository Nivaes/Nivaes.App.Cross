// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross.IoC
{
    using System;
    using MvvmCross.IoC;

    internal class ConstructingOpenGenericResolver
        : IResolver
    {
        private readonly Type mGenericTypeDefinition;
        private readonly IMvxIoCProvider mParent;

        private Type[]? mGenericTypeParameters;

        public ConstructingOpenGenericResolver(Type genericTypeDefinition, IMvxIoCProvider parent)
        {
            mGenericTypeDefinition = genericTypeDefinition;
            mParent = parent;
        }

        public void SetGenericTypeParameters(Type[] genericTypeParameters)
        {
            mGenericTypeParameters = genericTypeParameters;
        }

        public object Resolve()
        {
            return mParent.IoCConstruct(mGenericTypeDefinition.MakeGenericType(mGenericTypeParameters!));
        }

        public ResolverType ResolveType => ResolverType.DynamicPerResolve;
    }
}
