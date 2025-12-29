using System.Diagnostics.CodeAnalysis;
using Android.Runtime;
using Nivaes.App.Cross.Droid;
using Playground.Droid;

namespace Nivaes.App.Cross.Sample.Droid;

[Application()]
[RequiresUnreferencedCode("Uses MvvmCross reflection based plugin loading")]
public class MainApplication 
    : CrossAndroidApplication<Setup, SampleApp>
{
    public MainApplication(IntPtr javaReference, JniHandleOwnership transfer) 
        : base(javaReference, transfer)
    {
    }
}
