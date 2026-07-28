namespace Nivaes.App.Cross.Droid
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class DetailPresentationAttribute
        : ViewPagerFragmentPresentationAttribute
    {
        public DetailPresentationAttribute()
        {
            FragmentContentId = Resource.Id.detail_frame;
            AlternativeDetailActivityHostViewModelType = typeof(MainDetailViewModel);
        }

        /// <summary>
        /// Alternative Fragment parent activity ViewModel Type from detail. This activity is shown if the current hosting activity viewmodel is different.
        /// </summary>
        public Type? AlternativeDetailActivityHostViewModelType { get; set; }
    }
}
