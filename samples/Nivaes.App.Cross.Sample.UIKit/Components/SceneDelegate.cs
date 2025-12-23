namespace Playground.iOS
{
    using System.Diagnostics.CodeAnalysis;
    using Nivaes.App.Cross.UIKit;

    [Register("SceneDelegate")]
    [RequiresUnreferencedCode("Uses MvvmCross reflection based plugin loading")]
#pragma warning disable IL2026 // Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code
    public class SceneDelegate
            : MvxSceneDelegate<Setup, Playground.Core.App>;
#pragma warning restore IL2026 // Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code
}