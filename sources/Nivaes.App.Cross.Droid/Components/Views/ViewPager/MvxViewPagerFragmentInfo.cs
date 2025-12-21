using Fragment = AndroidX.Fragment.App.Fragment;

namespace MvvmCross.Platforms.Android.Views.ViewPager
{
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross;   

    public class MvxViewPagerFragmentInfo
    {
        public MvxViewPagerFragmentInfo(string title, string tag, Type fragmentType, CrossViewModelRequest request)
        {
            Title = title;
            Tag = tag;
            FragmentType = fragmentType;
            Request = request;
        }

        public Type FragmentType { get; }

        public string Tag { get; }

        public string Title { get; }

        public CrossViewModelRequest Request { get; }

        public Fragment CachedFragment { get; set; }
    }
}
