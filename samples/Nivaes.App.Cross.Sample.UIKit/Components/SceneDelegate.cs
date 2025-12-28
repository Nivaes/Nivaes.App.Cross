using System.Diagnostics.CodeAnalysis;
using Nivaes.App.Cross.UIKitOS;

namespace Nivaes.App.Cross.Sample.UIKitOS
{
    [Register("SceneDelegate")]
    [RequiresUnreferencedCode("Uses MvvmCross reflection based plugin loading")]
#pragma warning disable IL2026 // Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code
    public class SceneDelegate
            : MvxSceneDelegate<Setup, Nivaes.App.Cross.Sample.App>;
#pragma warning restore IL2026 // Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code
}