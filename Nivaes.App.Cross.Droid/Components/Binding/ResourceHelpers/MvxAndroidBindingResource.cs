// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.Platforms.Android.Binding.ResourceHelpers
{
    using System.Collections.Generic;
    using Nivaes.App.Cross.Droid;

    public class MvxAndroidBindingResource
        : IMvxAndroidBindingResource
    {
        public int BindingTagUnique => Resource.Id.MvxBindingTagUnique;

        public IEnumerable<int> BindingStylableGroupId => Resource.Styleable.MvxBinding;
        public int BindingBindId => Resource.Styleable.MvxBinding_MvxBind;
        public int BindingLangId => Resource.Styleable.MvxBinding_MvxLang;

        public IEnumerable<int> ControlStylableGroupId => Resource.Styleable.MvxControl;
        public int TemplateId => Resource.Styleable.MvxControl_MvxTemplate;

        public IEnumerable<int> ListViewStylableGroupId => Resource.Styleable.MvxListView;
        public int ListItemTemplateId => Resource.Styleable.MvxListView_MvxItemTemplate;
        public int DropDownListItemTemplateId => Resource.Styleable.MvxListView_MvxDropDownItemTemplate;

        public IEnumerable<int> ExpandableListViewStylableGroupId => Resource.Styleable.MvxExpandableListView;
        public int GroupItemTemplateId => Resource.Styleable.MvxExpandableListView_MvxGroupItemTemplate;
    }
}
