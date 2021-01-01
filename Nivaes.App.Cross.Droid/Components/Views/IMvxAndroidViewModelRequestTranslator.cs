// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Android.Content;

namespace MvvmCross.Platforms.Android.Views
{
    using Nivaes.App.Cross.ViewModels;

    public interface IMvxAndroidViewModelRequestTranslator
    {
        Intent GetIntentFor(MvxViewModelRequest request);

        // Important: if calling GetIntentWithKeyFor then you must later call RemoveSubViewModelWithKey on the returned key
        (Intent intent, int key) GetIntentWithKeyFor(IMvxViewModel existingViewModelToUse, MvxViewModelRequest request);

        void RemoveSubViewModelWithKey(int key);
    }
}
