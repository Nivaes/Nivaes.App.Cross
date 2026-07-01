using System.Diagnostics.CodeAnalysis;
using Nivaes.App.Cross.Hosting;
using Nivaes.App.Cross.UIKitOS;

namespace Nivaes.App.Cross.Sample.UIKitOS
{
    [Register("SceneDelegate")]
    [RequiresUnreferencedCode("Uses Cross reflection based plugin loading")]
    public class SceneDelegate : CrossSceneDelegate
    {
        protected override CrossApp CreateCrossApp(UIWindow window) => CrossProgram.CreateCrossApp(window);

        protected override void RegisterServices(IServiceProvider services)
        {
            base.RegisterServices(services);

            services
               .TargetBindingFactoryRegistry();
        }
    }
}