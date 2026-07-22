using Android.Runtime;
using Nivaes.App.Cross.Droid;
using Nivaes.App.Cross.Hosting;

namespace Nivaes.App.Cross.Sample.Droid;

[Application(
#if DEBUG
    UsesCleartextTraffic = true
#endif
)]
public class MainApplication
    : CrossDroidApplication
{
    public MainApplication(IntPtr javaReference, JniHandleOwnership transfer)
        : base(javaReference, transfer)
    {
    }

    protected override CrossApp CreateCrossApp() => CrossProgram.CreateCrossApp(this);

    public override void OnCreate()
    {
        base.OnCreate();
    }

    protected override void RegisterServices()
    {
        base.RegisterServices();

        ServiceProvider.TargetBindingFactoryRegistry();
    }

    protected override void RegisterViewsActions()
    {
        base.RegisterViewsActions();
        GeneratedViewsExtensions.RegisterViewsActions();
    }
}
