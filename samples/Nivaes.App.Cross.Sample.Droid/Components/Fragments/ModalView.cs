using System.Diagnostics.CodeAnalysis;
using Android.Runtime;
using Android.Views;
using Nivaes.App.Cross.Droid;
using Nivaes.IoC;
using Playground.Core.ViewModels;

namespace Nivaes.App.Cross.Sample.Droid;

[MvxDialogFragmentPresentation]
[RequiresUnreferencedCode("MvxBindings requires unreferenced code")]
public class ModalView : MvxDialogFragment<ModalViewModel>
{
    public ModalView()
    {
    }

    protected ModalView(IntPtr javaReference, JniHandleOwnership transfer)
        : base(javaReference, transfer)
    {
    }

    public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
    {
        var ignore = base.OnCreateView(inflater, container, savedInstanceState);

        var view = this.BindingInflate(Resource.Layout.ChildView, container, false);

        return view;
    }

    public override void OnPause()
    {
        var top = Mvx.IoCProvider.Resolve<IMvxAndroidCurrentTopActivity>();
        var activity = top.Activity;

        base.OnPause();
    }
}
