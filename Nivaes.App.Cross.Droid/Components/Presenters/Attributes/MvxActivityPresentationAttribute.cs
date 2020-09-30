// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Android.OS;

namespace Nivaes.App.Cross.Presenters
{
    using System;

    [AttributeUsage(AttributeTargets.Class)]
    public class MvxActivityPresentationAttribute
        : MvxBasePresentationAttribute
    {
        /// <summary>
        /// Add extras to the Intent that will be started for this Activity
        /// </summary>
        public Bundle? Extras { get; set; }
    }
}
