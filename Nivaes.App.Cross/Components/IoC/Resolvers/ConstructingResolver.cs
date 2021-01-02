// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross.IoC
{
    using System;
    using MvvmCross.IoC;

    internal class ConstructingResolver
        : IResolver
    {
        private readonly Type _type;
        private readonly IMvxIoCProvider _parent;

        public ConstructingResolver(Type type, IMvxIoCProvider parent)
        {
            _type = type;
            _parent = parent;
        }

        public object Resolve()
        {
            return _parent.IoCConstruct(_type);
        }

        public void SetGenericTypeParameters(Type[] genericTypeParameters)
        {
            throw new InvalidOperationException("This Resolver does not set generic type parameters");
        }

        public ResolverType ResolveType => ResolverType.DynamicPerResolve;
    }
}
