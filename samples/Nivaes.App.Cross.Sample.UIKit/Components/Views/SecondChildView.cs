using System.Diagnostics.CodeAnalysis;
using Nivaes.App.Cross.Sample;
using Nivaes.App.Cross.UIKitOS;
using ObjCRuntime;

namespace Nivaes.App.Cross.Sample.UIKitOS;

[MvxFromStoryboard("Main")]
[RequiresUnreferencedCode("Bindings require unreferenced code")]
public partial class SecondChildView : MvxViewController<SecondChildViewModel>
{
    public SecondChildView(NativeHandle handle) : base(handle)
    {
    }

    public override void ViewDidLoad()
    {
        base.ViewDidLoad();

        View?.BackgroundColor = UIColor.Green;

        var set = CreateBindingSet();
        set.Bind(btnClose).To(vm => vm.CloseCommand);
        set.Apply();
    }

    public override void ViewWillAppear(bool animated)
    {
        base.ViewWillAppear(animated);

        btnCloseStack.TouchUpInside += BtnCloseStack_TouchUpInside;
    }

    public override void ViewDidDisappear(bool animated)
    {
        base.ViewDidDisappear(animated);

        btnCloseStack.TouchUpInside -= BtnCloseStack_TouchUpInside;
    }

    private void BtnCloseStack_TouchUpInside(object sender, EventArgs e)
    {
        var appDelegate = UIApplication.SharedApplication.Delegate as UIApplicationDelegate;
        throw new NotImplementedException();    
        //var presenter = Mvx.IoCProvider.GetSingleton<IMvxIosViewPresenter>() as MvxIosViewPresenter;

        //if (appDelegate.Window.RootViewController.PresentedViewController != null)
        //{
        //    appDelegate.Window.RootViewController.DismissViewController(true, null);
        //    presenter.CloseModalViewControllers();
        //}
        //else
        //{
        //    presenter.MasterNavigationController.PopToRootViewController(true);
        //}
    }
}
