// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.Plugin.Color.Platforms.Wpf
{
    using Microsoft.UI.Xaml.Media;
    using MvvmCross.UI;

    public class MvxWpfColor : IMvxNativeColor
    {
        public object ToNative(System.Drawing.Color color)
        {
            var wpfColor = ToNativeColor(color);
            return new SolidColorBrush(wpfColor);
        }

        public static Windows.UI.Color ToNativeColor(System.Drawing.Color mvxColor)
        {
            return Windows.UI.Color.FromArgb(mvxColor.A, mvxColor.R, mvxColor.G, mvxColor.B);
        }
    }
}
