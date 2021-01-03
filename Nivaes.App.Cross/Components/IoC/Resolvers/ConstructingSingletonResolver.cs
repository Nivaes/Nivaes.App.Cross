// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross.IoC
{
    using System;

    [Obsolete("Sustituir por Autofac", true)]
    internal class ConstructingSingletonResolver
        : IResolver
    {
        private readonly object mSyncObject = new object();
        private readonly Func<object> mConstructor;
        private object? mTheObject;

        public ConstructingSingletonResolver(Func<object> theConstructor)
        {
            mConstructor = theConstructor;
        }

        public object Resolve()
        {
            if (mTheObject != null)
                return mTheObject;

            lock (mSyncObject)
            {
                if (mTheObject == null)
                    mTheObject = mConstructor();
            }

            return mTheObject;
        }

        public void SetGenericTypeParameters(Type[] genericTypeParameters)
        {
            throw new InvalidOperationException("This Resolver does not set generic type parameters");
        }

        public ResolverType ResolveType => ResolverType.Singleton;
    }
}
