using System.Diagnostics.CodeAnalysis;
using Android.Views;
using Nivaes.App.Cross.Droid;

namespace Nivaes.App.Cross.Sample.Droid
{
    //[MvxFragmentPresentation(typeof(RootViewModel), Resource.Id.content_frame, true)]
    [FragmentPresentation(typeof(RootViewModel), Resource.Id.content_frame, true, popBackStackImmediateName: null, popBackStackImmediateFlag: PopBackStack.None)]
    [RequiresUnreferencedCode("Uses Bindings which require unreferenced code")]
    internal sealed class FragmentCloseView : MvxFragment<FragmentCloseViewModel>
    {
        public override View? OnCreateView(LayoutInflater inflater, ViewGroup? container, Bundle? savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            return this.BindingInflate(Resource.Layout.FragmnetCloseView, container, false);
        }
    }
}
