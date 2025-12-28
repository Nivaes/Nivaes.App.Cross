using System.Diagnostics.CodeAnalysis;
using Android.Transitions;
using Android.Views;
using Nivaes.App.Cross.Droid;

namespace Nivaes.App.Cross.Sample.Droid
{
    [MvxFragmentPresentation(typeof(SharedElementRootViewModel), Resource.Id.shared_content_frame, true)]
    [RequiresUnreferencedCode("MvxBindings requires unreferenced code")]
    public class SharedElementSecondChildView : MvxFragment<SharedElementSecondChildViewModel>
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SharedElementEnterTransition = TransitionInflater.From(Activity).InflateTransition(Android.Resource.Transition.Move);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = this.BindingInflate(Resource.Layout.SharedElementSecondChildView, null);
            Arguments.SetSharedElementsById(view);

            return view;
        }
    }
}
