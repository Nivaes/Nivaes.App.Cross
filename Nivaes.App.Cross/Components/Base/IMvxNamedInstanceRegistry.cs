// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.Base
{

    using System;
    using System.Reflection;

    [Obsolete("Not user reflector")]
    public interface IMvxNamedInstanceRegistry<in T>
    {
        void AddOrOverwrite(string name, T instance);

        void AddOrOverwriteFrom(Assembly assembly);
    }
}
