// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.Platforms.Uap.Views
{
    using Nivaes.App.Cross.ViewModels;

    public interface IMvxWindowsViewModelRequestTranslator
    {
        string GetRequestTextFor(MvxViewModelRequest request);

        // Important: if calling GetRequestTextWithKeyFor then you must later call RemoveSubViewModelWithKey on the returned key
        string GetRequestTextWithKeyFor(IMvxViewModel existingViewModelToUse);

        void RemoveSubViewModelWithKey(int key);

        int RequestTextGetKey(string requestText);
    }
}
