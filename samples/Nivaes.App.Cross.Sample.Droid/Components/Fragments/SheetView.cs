using System.Diagnostics.CodeAnalysis;
using Android.Runtime;
using Android.Views;
using Nivaes.App.Cross.Droid;

namespace Nivaes.App.Cross.Sample.Droid;

[MvxDialogFragmentPresentation]
[RequiresUnreferencedCode("Bindings require unreferenced code")]
public class SheetView : MvxBottomSheetDialogFragment<SheetViewModel>
{
    public SheetView()
    {
    }

    protected SheetView(IntPtr javaReference, JniHandleOwnership transfer)
        : base(javaReference, transfer)
    {
    }

    public override View OnCreateView(LayoutInflater inflater, ViewGroup? container, Bundle? savedInstanceState)
    {
        base.OnCreateView(inflater, container, savedInstanceState);

        var view = this.BindingInflate(Resource.Layout.SheetView, container, false);

        return view;
    }
}
