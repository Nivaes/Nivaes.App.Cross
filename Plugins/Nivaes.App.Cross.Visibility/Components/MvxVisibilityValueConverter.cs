// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.Plugin.Visibility
{
    using System.Globalization;
    using MvvmCross.Base;
    using MvvmCross.UI;

    [Preserve(AllMembers = true)]
    public class MvxVisibilityValueConverter : MvxBaseVisibilityValueConverter
    {
        protected override MvxVisibility Convert(object? value, object? parameter, CultureInfo culture)
        {
            bool? visible = value?.ConvertToBooleanCore();
            bool? hide = parameter?.ConvertToBooleanCore();

            if (visible == false)
            {
                return hide == true ? MvxVisibility.Hidden : MvxVisibility.Collapsed;
            }

            return MvxVisibility.Visible;
        }
    }
}
