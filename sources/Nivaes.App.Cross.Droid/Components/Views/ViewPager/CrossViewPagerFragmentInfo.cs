namespace Nivaes.App.Cross.Droid
{
    using MvvmCross.ViewModels;
    using Fragment = AndroidX.Fragment.App.Fragment;

    public class CrossViewPagerFragmentInfo
    {
        public CrossViewPagerFragmentInfo(string title, string tag, Type fragmentType, ICrossViewModelRequest request)
        {
            Title = title;
            Tag = tag;
            FragmentType = fragmentType;
            Request = request;
        }

        public Type FragmentType { get; }

        public string Tag { get; }

        public string Title { get; }

        public ICrossViewModelRequest Request { get; }

        public Fragment CachedFragment { get; set; }
    }
}
