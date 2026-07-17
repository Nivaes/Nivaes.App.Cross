namespace Nivaes.App.Cross.Droid
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class DefaultDetailPresentationAttribute
        : ViewPagerFragmentPresentationAttribute
    {
        public DefaultDetailPresentationAttribute()
        {
            FragmentContentId = Resource.Id.detail_frame;
        }

        public int FragmentContentId { get; set; }
    }
}
