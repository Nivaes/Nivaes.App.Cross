// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.Platforms.Tizen.Views
{
    using MvvmCross.Views;
    using Nivaes.App.Cross.ViewModels;

    public class MvxTizenViewsContainer
        : MvxViewsContainer
        , IMvxTizenViewsContainer
    {
        public MvxViewModelRequest CurrentRequest { get; private set; }
    }
}
