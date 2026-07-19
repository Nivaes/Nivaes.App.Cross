namespace Nivaes.App.Cross.Droid
{
    using Fragment = AndroidX.Fragment.App.Fragment;

    public class MvxViewPagerFragmentInfo
    {
        public MvxViewPagerFragmentInfo(string? title, string? tag, Type? fragmentType, ViewModelRequest request)
        {
            Title = title;
            Tag = tag;
            FragmentType = fragmentType;
            Request = request;
        }

        public Type FragmentType { get; }

        public string Tag { get; }

        public string Title { get; }

        public ViewModelRequest Request { get; }

        public Fragment? CachedFragment { get; set; }
    }
}
