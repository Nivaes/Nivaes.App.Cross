namespace Nivaes.App.Cross.Droid
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class FullPresentationAttribute
        : ViewPagerFragmentPresentationAttribute
    {
        public FullPresentationAttribute(PanelType panelType = PanelType.Primary)
        {
            base.FragmentContentId = Resource.Id.content_frame;

            switch (panelType)
            {
                case PanelType.Primary:
                    base.ActivityHostViewModelType = typeof(PrimaryViewModel);
                    FragmentHostFullView = typeof(PrimaryActivity);
                    break;
                case PanelType.Secondary:
                    base.ActivityHostViewModelType = typeof(SecondaryViewModel);
                    FragmentHostFullView = typeof(SecondaryActivity);
                    break;
            }
        }

        public Type? FragmentHostFullView { get; set; }
    }
}
