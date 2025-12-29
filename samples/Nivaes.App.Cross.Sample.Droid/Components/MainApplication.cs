using System.Diagnostics.CodeAnalysis;
using Android.Runtime;
using Nivaes.App.Cross.Droid;
using Playground.Droid;

namespace Nivaes.App.Cross.Sample.Droid;

[Application()]
[RequiresUnreferencedCode("Uses MvvmCross reflection based plugin loading")]
#pragma warning disable IL2026 // Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code
public class MainApplication 
    : MvxAndroidApplication<Setup, SampleApp>
#pragma warning restore IL2026 // Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code
{
    public MainApplication(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
    {
    }
}
