// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.Platforms.Mac.Presenters.Attributes
{
    using Nivaes.App.Cross.Presenters;

    public class MvxModalPresentationAttribute
        : MvxBasePresentationAttribute
    {
        public string WindowIdentifier { get; set; } = string.Empty;
    }
}
