// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.Platforms.Tvos.Presenters.Attributes
{
    using Nivaes.App.Cross.Presenters;

    public class MvxChildPresentationAttribute
        : MvxBasePresentationAttribute
    {
        public static bool DefaultAnimated = true;
        public bool Animated { get; set; } = DefaultAnimated;
    }
}
