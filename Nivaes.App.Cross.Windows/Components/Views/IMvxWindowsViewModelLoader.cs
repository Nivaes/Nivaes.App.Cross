// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.Platforms.Uap.Views
{
    using System.Threading.Tasks;
    using Nivaes.App.Cross.ViewModels;

    public interface IMvxWindowsViewModelLoader
    {
        ValueTask<IMvxViewModel> Load(string requestText, IMvxBundle? savedState);
    }
}
