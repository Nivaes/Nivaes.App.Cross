// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.Plugin.Visibility.Platforms.Uap
{
    using MvvmCross.UI;
    using UapVisibility = Microsoft.UI.Xaml.Visibility;

    public class MvxWinRTVisibility : IMvxNativeVisibility
    {
        public object ToNative(MvxVisibility visibility)
        {
            return visibility == MvxVisibility.Visible
                       ? UapVisibility.Visible
                       : UapVisibility.Collapsed;
        }
    }
}
