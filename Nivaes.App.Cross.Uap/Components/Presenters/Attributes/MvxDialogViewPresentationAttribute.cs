// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.Platforms.Uap.Presenters.Attributes
{
    using Microsoft.UI.Xaml.Controls;
    using Nivaes.App.Cross.Presenters;

    public class MvxDialogViewPresentationAttribute
        : MvxBasePresentationAttribute
    {
        public MvxDialogViewPresentationAttribute()
        {
        }

        public ContentDialogPlacement Placement { get; set; }
    }
}
