namespace Playground.Droid.Fragments
{
    using System.Diagnostics.CodeAnalysis;
    using Android.Views;
    using MvvmCross.Platforms.Android.Binding.BindingContext;
    using MvvmCross.Platforms.Android.Presenters.Attributes;
    using MvvmCross.Platforms.Android.Views.Fragments;
    using MvvmCross.Presenters.Attributes;
    using Nivaes.App.Cross;
    using Playground.Core.ViewModels;
    using Resource = Nivaes.App.Cross.Sample.Droid.Resource;

    [MvxFragmentPresentation(typeof(RootViewModel), Resource.Id.content_frame)]
    [MvxFragmentPresentation(typeof(SplitRootViewModel), Resource.Id.split_content_frame)]
    [RequiresUnreferencedCode("MvxBindings requires unreferenced code")]
    public class OverrideAttributeView : MvxFragment<OverrideAttributeViewModel>, ICrossOverridePresentationAttribute
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = this.BindingInflate(Resource.Layout.ChildView, container, false);

            return view;
        }

        public MvxBasePresentationAttribute PresentationAttribute(CrossViewModelRequest request)
        {
            return new MvxFragmentPresentationAttribute(
                typeof(RootViewModel),
                Resource.Id.content_frame,
                false,
                Resource.Animation.abc_fade_in,
                Resource.Animation.abc_fade_out,
                Resource.Animation.abc_fade_in,
                Resource.Animation.abc_fade_out);
        }
    }
}
