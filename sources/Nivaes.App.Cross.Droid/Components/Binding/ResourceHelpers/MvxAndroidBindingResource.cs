namespace Nivaes.App.Cross.Droid
{
    public class MvxAndroidBindingResource : IMvxAndroidBindingResource
    {
        public int BindingTagUnique => Resource.Id.MvxBindingTagUnique;

        public int[] BindingStylableGroupId => Resource.Styleable.MvxBinding;
        public int BindingBindId => Resource.Styleable.MvxBinding_MvxBind;
        public int BindingLangId => Resource.Styleable.MvxBinding_MvxLang;

        public int[] ControlStylableGroupId => Resource.Styleable.MvxControl;
        public int TemplateId => Resource.Styleable.MvxControl_MvxTemplate;

        public int[] ListViewStylableGroupId => Resource.Styleable.MvxListView;
        public int ListItemTemplateId => Resource.Styleable.MvxListView_MvxItemTemplate;
        public int DropDownListItemTemplateId => Resource.Styleable.MvxListView_MvxDropDownItemTemplate;

        public int[] ExpandableListViewStylableGroupId => Resource.Styleable.MvxExpandableListView;
        public int GroupItemTemplateId => Resource.Styleable.MvxExpandableListView_MvxGroupItemTemplate;
    }
}
