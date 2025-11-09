namespace Nivaes.App.Cross.Droid
{
    using Android;

    public class CrossAndroidBindingResource : ICrossAndroidBindingResource
    {
        public int BindingTagUnique => Resource.Id.CrossBindingTagUnique;

        public int[] BindingStylableGroupId => Resource.Styleable.CrossBinding;
        public int BindingBindId => Resource.Styleable.CrossBinding_CrossBind;
        public int BindingLangId => Resource.Styleable.CrossBinding_CrossLang;

        public int[] ControlStylableGroupId => Resource.Styleable.CrossControl;
        public int TemplateId => Resource.Styleable.CrossControl_CrossTemplate;

        public int[] ListViewStylableGroupId => Resource.Styleable.CrossListView;
        public int ListItemTemplateId => Resource.Styleable.CrossListView_CrossItemTemplate;
        public int DropDownListItemTemplateId => Resource.Styleable.CrossListView_CrossDropDownItemTemplate;

        public int[] ExpandableListViewStylableGroupId => Resource.Styleable.CrossExpandableListView;
        public int GroupItemTemplateId => Resource.Styleable.CrossExpandableListView_CrossGroupItemTemplate;
    }
}
