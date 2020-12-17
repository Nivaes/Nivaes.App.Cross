// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.Plugin.Visibility
{
    using System.Globalization;
    using MvvmCross.UI;
    using Nivaes.App.Cross;

    [Preserve(AllMembers = true)]
    public class MvxInvertedVisibilityValueConverter : MvxVisibilityValueConverter
    {
        protected override MvxVisibility Convert(object? value, object? parameter, CultureInfo culture)
        {
            bool hide = parameter?.ConvertToBooleanCore() == true;

            return (base.Convert(value, parameter, culture)) switch
            {
                MvxVisibility.Visible when hide => MvxVisibility.Hidden,
                MvxVisibility.Visible when !hide => MvxVisibility.Collapsed,
                _ => MvxVisibility.Visible,
            };
        }
    }
}
