using System.Diagnostics.CodeAnalysis;
using Android.Runtime;
using Android.Views;
using Microsoft.Extensions.DependencyInjection;
using Nivaes.App.Cross.Droid;

namespace Nivaes.App.Cross.Sample.Droid;

[DialogFragmentPresentation]
[RequiresUnreferencedCode("Bindings requires unreferenced code")]
public class ModalView : MvxDialogFragment<ModalViewModel>
{
    public ModalView()
    {
    }

    protected ModalView(IntPtr javaReference, JniHandleOwnership transfer)
        : base(javaReference, transfer)
    {
    }

    public override View? OnCreateView(LayoutInflater inflater, ViewGroup? container, Bundle? savedInstanceState)
    {
        var ignore = base.OnCreateView(inflater, container, savedInstanceState);

        var view = this.BindingInflate(Resource.Layout.ChildView, container, false);

        return view;
    }

    public override void OnPause()
    {
        var top = IPlatformApplication.Current!.Services.GetRequiredService<IMvxAndroidCurrentTopActivity>();
        var activity = top.Activity;

        base.OnPause();
    }
}
