// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable

using System.Diagnostics.CodeAnalysis;
using Android.Content;
using MvvmCross.ViewModels;

namespace Nivaes.App.Cross.Droid;

public interface ICrossAndroidViewModelLoader
{
    [RequiresUnreferencedCode("This method uses reflection which may not be preserved during trimming.")]
    ICrossViewModel? Load(Intent? intent, IMvxBundle? savedState);

    [RequiresUnreferencedCode("This method uses reflection which may not be preserved during trimming.")]
    ICrossViewModel? Load(
        Intent? intent,
        IMvxBundle? savedState,
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type? viewModelTypeHint);
}
