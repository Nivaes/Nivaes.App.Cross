// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Android.Content;

namespace MvvmCross.Platforms.Android.Views
{
    using System;
    using System.Threading.Tasks;
    using Nivaes.App.Cross.ViewModels;

    public interface IMvxAndroidViewModelLoader
    {
        ValueTask<IMvxViewModel?> Load(Intent intent, IMvxBundle? savedState);

        ValueTask<IMvxViewModel?> Load(Intent intent, IMvxBundle? savedState, Type? viewModelTypeHint);
    }
}
