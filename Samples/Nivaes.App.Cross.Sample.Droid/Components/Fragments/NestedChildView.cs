using System.Diagnostics.CodeAnalysis;
using Android.Views;
using Nivaes.App.Cross.Droid;

namespace Nivaes.App.Cross.Sample.Droid
{
    [FragmentPresentation(fragmentHostViewType: typeof(SecondChildView), fragmentContentId: Resource.Id.nested_frame)]
    [RequiresUnreferencedCode("Bindings requires unreferenced code")]
    public class NestedChildView : MvxFragment<NestedChildViewModel>
    {
        public override View? OnCreateView(LayoutInflater inflater, ViewGroup? container, Bundle? savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = this.BindingInflate(Resource.Layout.NestedChildView, container, false);

            return view;
        }
    }
}
