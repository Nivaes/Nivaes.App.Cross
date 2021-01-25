// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross.Droid
{
    using Android.OS;
    using Nivaes.App.Cross.ViewModels;

    public interface IMvxSavedStateConverter
    {
        IMvxBundle? Read(Bundle? bundle);

        void Write(Bundle? bundle, IMvxBundle? savedState);
    }
}
