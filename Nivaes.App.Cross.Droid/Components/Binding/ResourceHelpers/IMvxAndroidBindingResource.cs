// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.Platforms.Android.Binding.ResourceHelpers
{
    using System.Collections.Generic;

    public interface IMvxAndroidBindingResource
    {
        int BindingTagUnique { get; }

        IEnumerable<int> BindingStylableGroupId { get; }

        int BindingBindId { get; }

        int BindingLangId { get; }

        IEnumerable<int> ControlStylableGroupId { get; }

        int TemplateId { get; }

        IEnumerable<int> ListViewStylableGroupId { get; }

        int ListItemTemplateId { get; }

        int DropDownListItemTemplateId { get; }

        IEnumerable<int> ExpandableListViewStylableGroupId { get; }

        int GroupItemTemplateId { get; }
    }
}
