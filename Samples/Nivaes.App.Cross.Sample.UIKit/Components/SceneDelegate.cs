using System.Diagnostics.CodeAnalysis;
using Nivaes.App.Cross.Hosting;
using Nivaes.App.Cross.UIKitLib;

namespace Nivaes.App.Cross.Sample.UIKitLib
{
    [Register("SceneDelegate")]
    [RequiresUnreferencedCode("Uses Cross reflection based plugin loading")]
    public class SceneDelegate : CrossSceneDelegate
    {
        protected override CrossApp CreateCrossApp(UIWindow window) => CrossProgram.CreateCrossApp(window);

        protected override void RegisterServices()
        {
            base.RegisterServices();

            ServiceProvider
               .TargetBindingFactoryRegistry();
        }
    }
}