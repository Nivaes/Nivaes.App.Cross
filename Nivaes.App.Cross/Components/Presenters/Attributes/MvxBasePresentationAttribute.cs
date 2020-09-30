// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross.Presenters
{
    using System;

    [AttributeUsage(AttributeTargets.Class)]
    public abstract class MvxBasePresentationAttribute
        : Attribute, IMvxPresentationAttribute
    {
        public Type? ViewModelType { get; set; }

        public Type? ViewType { get; set; }
    }
}
