namespace Nivaes.App.Cross.Droid
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class MasterPresentationAttribute
        : ViewPagerFragmentPresentationAttribute
    {
        public MasterPresentationAttribute(PanelType panelType = PanelType.Primary)
        {
            base.FragmentContentId = Resource.Id.master_frame;

            switch (panelType)
            {
                case PanelType.Primary:
                    FragmentHostMasterDetailViewModelType = typeof(PrimaryMasterDetailViewModel);
                    FragmentHostMasterDetailViewType = typeof(PrimaryMasterDetailView);
                    ActivityHostViewModelType = typeof(PrimaryViewModel);
                    break;
                case PanelType.Secondary:
                    FragmentHostMasterDetailViewModelType = typeof(SecondaryMasterDetailViewModel);
                    FragmentHostMasterDetailViewType = typeof(SecondaryMasterDetailView);
                    ActivityHostViewModelType = typeof(SecondaryViewModel);
                    break;
            }
        }

        /// <summary>
        /// Fragment parent ViewModel Type from detail. This activity is shown if the current hosting activity viewmodel is different.
        /// </summary>
        public Type FragmentHostMasterDetailViewModelType { get; set; }

        /// <summary>
        /// Fragment parent ViewModel Type from detail. This activity is shown if the current hosting activity viewmodel is different.
        /// </summary>
        public Type FragmentHostMasterDetailViewType { get; set; }
    }
}
