// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross.IoC
{
    using System;

    internal class FuncConstructingResolver
        : IResolver
    {
        private readonly Func<object> mConstructor;

        public FuncConstructingResolver(Func<object> constructor)
        {
            mConstructor = constructor;
        }

        public object Resolve()
        {
            return mConstructor();
        }

        public void SetGenericTypeParameters(Type[] genericTypeParameters)
        {
            throw new InvalidOperationException("This Resolver does not set generic type parameters");
        }

        public ResolverType ResolveType => ResolverType.DynamicPerResolve;
    }
}
